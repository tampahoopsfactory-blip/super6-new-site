using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.DeviceSdk.Abstractions;
using AccessControl.DeviceSdk.Models;
using DoNetDrive.Core;
using DoNetDrive.Core.Command;
using DoNetDrive.Core.Connector;
using DoNetDrive.Core.Factory;
using DoNetDrive.Protocol.Door.Door8800;
using DoNetDrive.Protocol.Door.Door8800.Data;
using DoNetDrive.Protocol.Door.Door8800.SystemParameter.SN;
using DoNetDrive.Protocol.Door.Door8800.Time;
using DoNetDrive.Protocol.Door.Door8800.Transaction;
using DoNetDrive.Protocol;
using DoNetDrive.Protocol.Fingerprint.AdditionalData;
using DoNetDrive.Protocol.Fingerprint.Person;
using DoNetDrive.Protocol.Fingerprint.Door.Remote;

namespace AccessControl.DeviceSdk.Native;

/// <summary>
/// Native DS-F881 device client leveraging the official DoNetDrive SDK.
/// </summary>
internal sealed class DsF881DeviceClient : IDeviceClient
{
    private const int DefaultTransactionBatchSize = 200;
    private const int MaxFaceImageBytes = 122_880; // per vendor guidance (480x640 JPEG)

    private readonly DeviceConnectionOptions _options;
    private readonly ConnectorAllocator _allocator;
    private readonly SemaphoreSlim _commandGate = new(1, 1);
    private readonly ConcurrentDictionary<Guid, uint> _userCodeByUserId = new();
    private readonly ConcurrentDictionary<uint, Guid> _userIdByUserCode = new();
    private readonly ConcurrentDictionary<Guid, DeviceUser> _userSnapshot = new();

    private INCommandDetail? _commandDetail;
    private bool _disposed;
    private bool _connected;
    private long _lastTransactionPointer;

    public DsF881DeviceClient(DeviceConnectionOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        ValidateOptions(_options);
        _allocator = ConnectorAllocator.GetAllocator();
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();
        if (_connected)
        {
            return;
        }

        var detail = GetOrCreateCommandDetail();
        _allocator.OpenForciblyConnect(detail.Connector);

        var result = await ExecuteWithResultAsync<SN_Result>(d => new ReadSN(d), cancellationToken).ConfigureAwait(false);
        var deviceSn = Encoding.ASCII.GetString(result.SNBuf).TrimEnd('\0', ' ');

        if (!string.IsNullOrWhiteSpace(_options.SerialNumber) &&
            !deviceSn.Equals(_options.SerialNumber, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Connected device SN '{deviceSn}' does not match configured SN '{_options.SerialNumber}'.");
        }

        _connected = true;
    }

    public async Task EnsureTimeSyncAsync(CancellationToken cancellationToken = default)
    {
        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);

        var timeResult = await ExecuteWithResultAsync<ReadTime_Result>(d => new ReadTime(d), cancellationToken).ConfigureAwait(false);
        var controllerTime = DateTime.SpecifyKind(timeResult.ControllerDate, DateTimeKind.Unspecified);
        var controllerUtc = ToUtc(controllerTime);
        var nowUtc = DateTimeOffset.UtcNow;

        if (Math.Abs((controllerUtc - nowUtc).TotalSeconds) <= 2)
        {
            return;
        }

        var parameter = new WriteCustomTime_Parameter(nowUtc.UtcDateTime);
        await ExecuteAsync(d => new WriteCustomTime(d, parameter), cancellationToken).ConfigureAwait(false);
    }

    public async Task UpsertUserAsync(DeviceUser user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);

        var userCode = ReserveUserCode(user.Id);
        var person = CreatePersonEnvelope(user, userCode);
        var payload = new List<DoNetDrive.Protocol.Fingerprint.Data.Person> { person };
        var parameter = new AddPerson_Parameter(payload);

        try
        {
            await ExecuteWithResultAsync<WritePerson_Result>(d => new AddPerson(d, parameter), cancellationToken).ConfigureAwait(false);
            _userSnapshot[user.Id] = user;
        }
        catch
        {
            // Roll back user code reservation if the device rejects the update.
            RemoveUserCodeReservation(user.Id, userCode);
            throw;
        }
    }

    public async Task UploadFaceAsync(Guid userId, byte[] jpegBytes, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(jpegBytes);
        if (jpegBytes.Length == 0)
        {
            throw new ArgumentException("Face image payload cannot be empty.", nameof(jpegBytes));
        }
        if (jpegBytes.Length > MaxFaceImageBytes)
        {
            throw new ArgumentException($"Face image exceeds {MaxFaceImageBytes} bytes. Resize or compress before uploading.", nameof(jpegBytes));
        }

        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);

        var userCode = ReserveUserCode(userId);
        var person = _userSnapshot.TryGetValue(userId, out var snapshot)
            ? CreatePersonEnvelope(snapshot, userCode)
            : CreateFallbackPerson(userCode);

        var identification = new IdentificationData(1, jpegBytes);
        var parameter = new AddPersonAndImage_Parameter(person, new[] { identification }, bWaitRepeatMessage: true);

        var result = await ExecuteWithResultAsync<AddPersonAndImage_Result>(d => new AddPeosonAndImage(d, parameter), cancellationToken).ConfigureAwait(false);
        if (!result.UserUploadStatus || result.IdDataUploadStatus.Any(status => status is not 1))
        {
            throw new InvalidOperationException("Device reported failure uploading face image.");
        }
    }

    public async Task UploadPalmTemplateAsync(Guid userId, BiometricTemplate template, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(template);
        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);

        var userCode = ReserveUserCode(userId);
        var fileType = template.TemplateType switch
        {
            BiometricTemplateType.Palm => 3,
            BiometricTemplateType.Fingerprint => 2,
            _ => 2
        };
        var fileNum = template.TemplateType == BiometricTemplateType.Fingerprint ? 0 : 1;

        var parameter = new WriteFeatureCode_Parameter((int)userCode, fileType, fileNum, template.Data)
        {
            WaitRepeatMessage = true
        };

        var result = await ExecuteWithResultAsync<WriteFeatureCode_Result>(d => new WriteFeatureCode(d, parameter), cancellationToken).ConfigureAwait(false);
        if (result.Result != 1)
        {
            throw new InvalidOperationException($"Device rejected biometric template upload (result code {result.Result}).");
        }
    }

    public async Task RemoveUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);

        if (!_userCodeByUserId.TryGetValue(userId, out var userCode))
        {
            // Nothing to remove from the device, ensure local cache is clean.
            RemoveUserCodeReservation(userId, null);
            return;
        }

        var person = new DoNetDrive.Protocol.Fingerprint.Data.Person(userCode, string.Empty);
        var parameter = new DeletePerson_Parameter(new List<DoNetDrive.Protocol.Fingerprint.Data.Person> { person });

        await ExecuteAsync(d => new DeletePerson(d, parameter), cancellationToken).ConfigureAwait(false);
        RemoveUserCodeReservation(userId, userCode);
        _userSnapshot.TryRemove(userId, out _);
    }

    public async Task<IReadOnlyList<DeviceTransaction>> ReadTransactionsAsync(long sincePointer, CancellationToken cancellationToken = default)
    {
        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);

        var startIndex = sincePointer <= 0 ? 1 : (int)Math.Min(int.MaxValue, sincePointer + 1);
        var parameter = new ReadTransactionDatabaseByIndex_Parameter(1, startIndex, DefaultTransactionBatchSize);
        var result = await ExecuteWithResultAsync<ReadTransactionDatabaseByIndex_Result>(d => new ReadTransactionDatabaseByIndex(d, parameter), cancellationToken).ConfigureAwait(false);

        var transactions = new List<DeviceTransaction>();
        if (result.TransactionList == null || result.TransactionList.Count == 0)
        {
            return Array.Empty<DeviceTransaction>();
        }

        foreach (var transaction in result.TransactionList)
        {
            if (transaction == null || transaction.IsNull())
            {
                continue;
            }

            var sequence = transaction.SerialNumber;
            var occurredAtUtc = ToUtc(transaction.TransactionDate);
            Guid userId = Guid.Empty;

            if (transaction is CardTransaction cardTransaction)
            {
                var code = cardTransaction.CardData;
                if (_userIdByUserCode.TryGetValue(code, out var mappedUserId))
                {
                    userId = mappedUserId;
                }
            }

            transactions.Add(new DeviceTransaction(sequence, userId, occurredAtUtc));
            _lastTransactionPointer = Math.Max(_lastTransactionPointer, sequence);
        }

        return transactions;
    }

    public async Task ControlRelayAsync(int relayPort, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        await EnsureConnectedAsync(cancellationToken).ConfigureAwait(false);
        await ExecuteAsync(d => new OpenDoor(d), cancellationToken).ConfigureAwait(false);

        if (duration > TimeSpan.Zero)
        {
            try
            {
                await Task.Delay(duration, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Cancellation triggered while door is open – fall through to best-effort close.
            }

            await ExecuteAsync(d => new CloseDoor(d), CancellationToken.None).ConfigureAwait(false);
        }
    }

    public ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return ValueTask.CompletedTask;
        }

        _disposed = true;
        _connected = false;

        if (_commandDetail != null)
        {
            try
            {
                _allocator.CloseConnector(_commandDetail.Connector);
            }
            catch
            {
                // Ignored – connector allocator handles its own lifecycle.
            }
            _commandDetail = null;
        }

        _commandGate.Dispose();
        return ValueTask.CompletedTask;
    }

    private async Task EnsureConnectedAsync(CancellationToken cancellationToken)
    {
        if (!_connected)
        {
            await ConnectAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private INCommandDetail GetOrCreateCommandDetail()
    {
        if (_commandDetail != null)
        {
            return _commandDetail;
        }

        var detail = CommandDetailFactory.CreateDetail(
            CommandDetailFactory.ConnectType.TCPClient,
            _options.Host,
            _options.Port,
            CommandDetailFactory.ControllerType.Door89H,
            _options.SerialNumber,
            _options.CommunicationPassword);

        detail.Timeout = (int)Math.Clamp(_options.ConnectTimeout.TotalMilliseconds, 1000, 60000);
        detail.RestartCount = Math.Max(0, _options.RetryCount);
        _commandDetail = detail;
        return detail;
    }

    private async Task ExecuteAsync(Func<INCommandDetail, INCommand> commandFactory, CancellationToken cancellationToken)
    {
        await _commandGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var detail = GetOrCreateCommandDetail();
            var command = commandFactory(detail);
            await _allocator.AddCommandAsync(command).WaitAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _commandGate.Release();
        }
    }

    private async Task<TResult> ExecuteWithResultAsync<TResult>(Func<INCommandDetail, INCommand> commandFactory, CancellationToken cancellationToken)
        where TResult : class
    {
        await _commandGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var detail = GetOrCreateCommandDetail();
            var command = commandFactory(detail);
            await _allocator.AddCommandAsync(command).WaitAsync(cancellationToken).ConfigureAwait(false);

            if (command.getResult() is TResult typed)
            {
                return typed;
            }

            throw new InvalidOperationException($"Command {command.GetType().Name} did not return a {typeof(TResult).Name} result.");
        }
        finally
        {
            _commandGate.Release();
        }
    }

    private uint ReserveUserCode(Guid userId)
    {
        return _userCodeByUserId.GetOrAdd(userId, id => AllocateUserCode(id));
    }

    private uint AllocateUserCode(Guid userId)
    {
        Span<byte> guidBytes = stackalloc byte[16];
        userId.TryWriteBytes(guidBytes);

        Span<byte> hash = stackalloc byte[32];
        SHA256.HashData(guidBytes, hash);

        uint candidate = BitConverter.ToUInt32(hash);
        if (candidate == 0)
        {
            candidate = BitConverter.ToUInt32(hash[4..8]);
            if (candidate == 0)
            {
                candidate = 1;
            }
        }

        var attempt = 0;
        while (_userIdByUserCode.TryGetValue(candidate, out var existing) && existing != userId)
        {
            candidate = unchecked(candidate + 1);
            if (candidate == 0)
            {
                candidate = 1;
            }
            attempt++;
            if (attempt > 1024)
            {
                throw new InvalidOperationException("Unable to allocate a unique user code for DS-F881 device after 1024 attempts.");
            }
        }

        _userIdByUserCode[candidate] = userId;
        return candidate;
    }

    private void RemoveUserCodeReservation(Guid userId, uint? userCode)
    {
        if (userCode.HasValue)
        {
            _userIdByUserCode.TryRemove(userCode.Value, out _);
        }
        _userCodeByUserId.TryRemove(userId, out _);
    }

    private static DoNetDrive.Protocol.Fingerprint.Data.Person CreatePersonEnvelope(DeviceUser user, uint userCode)
    {
        var person = new DoNetDrive.Protocol.Fingerprint.Data.Person(userCode, user.DisplayName ?? string.Empty)
        {
            CardData = userCode,
            Password = string.Empty,
            Expiry = DateTime.UtcNow.AddYears(5),
            TimeGroup = 1,
            OpenTimes = 65_535,
            Identity = 0,
            CardType = 0,
            CardStatus = 0,
            EnterStatus = 3,
            PCode = user.TicketNumber ?? string.Empty,
            Dept = string.IsNullOrWhiteSpace(user.PhoneNumber) ? "EventShield" : user.PhoneNumber,
            Job = string.Empty,
            IsFaceFeatureCode = user.EnableFace,
            FingerprintFeatureCodeCout = user.EnablePalm ? 1 : 0
        };

        return person;
    }

    private static DoNetDrive.Protocol.Fingerprint.Data.Person CreateFallbackPerson(uint userCode)
    {
        var alias = $"User-{userCode}";
        return new DoNetDrive.Protocol.Fingerprint.Data.Person(userCode, alias)
        {
            CardData = userCode,
            Expiry = DateTime.UtcNow.AddYears(5),
            TimeGroup = 1,
            OpenTimes = 65_535,
            EnterStatus = 3
        };
    }

    private static DateTimeOffset ToUtc(DateTime timestamp)
    {
        if (timestamp.Kind == DateTimeKind.Utc)
        {
            return new DateTimeOffset(timestamp, TimeSpan.Zero);
        }

        if (timestamp.Kind == DateTimeKind.Local)
        {
            return new DateTimeOffset(timestamp).ToUniversalTime();
        }

        // Device timestamps are reported without a kind; treat them as local and convert to UTC.
        var assumedLocal = DateTime.SpecifyKind(timestamp, DateTimeKind.Local);
        return new DateTimeOffset(assumedLocal).ToUniversalTime();
    }

    private static void ValidateOptions(DeviceConnectionOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Host))
        {
            throw new ArgumentException("Device host must be specified.", nameof(options));
        }
        if (string.IsNullOrWhiteSpace(options.SerialNumber))
        {
            throw new ArgumentException("Device serial number must be specified.", nameof(options));
        }
        if (string.IsNullOrWhiteSpace(options.CommunicationPassword))
        {
            throw new ArgumentException("Device communication password must be specified.", nameof(options));
        }
    }

    private void EnsureNotDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(DsF881DeviceClient));
        }
    }
}
