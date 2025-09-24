using AccessControl.DeviceSdk.Abstractions;
using AccessControl.DeviceSdk.Models;

namespace AccessControl.DeviceSdk.Simulation;

public sealed class SimulatedDeviceClient : IDeviceClient
{
    private readonly Action<string>? _log;
    private readonly Dictionary<Guid, DeviceUser> _users = new();
    private readonly List<DeviceTransaction> _transactions = new();
    private bool _connected;
    private DateTimeOffset _lastSync = DateTimeOffset.MinValue;

    public SimulatedDeviceClient(Action<string>? logHandler = null)
    {
        _log = logHandler;
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        _connected = true;
        _log?.Invoke("Simulated device connected");
        return Task.CompletedTask;
    }

    public Task EnsureTimeSyncAsync(CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        _lastSync = DateTimeOffset.UtcNow;
        _log?.Invoke($"Simulated time sync at {_lastSync:O}");
        return Task.CompletedTask;
    }

    public Task UpsertUserAsync(DeviceUser user, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        _users[user.Id] = user;
        _transactions.Add(new DeviceTransaction(_transactions.Count + 1, user.Id, DateTimeOffset.UtcNow));
        _log?.Invoke($"Upserted simulated user {user.Id}");
        return Task.CompletedTask;
    }

    public Task UploadFaceAsync(Guid userId, byte[] jpegBytes, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        if (!_users.ContainsKey(userId))
        {
            throw new InvalidOperationException($"User {userId} must be created before uploading face data.");
        }

        _log?.Invoke($"Simulated face upload for {userId} ({jpegBytes.Length} bytes)");
        return Task.CompletedTask;
    }

    public Task UploadPalmTemplateAsync(Guid userId, BiometricTemplate template, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        if (!_users.ContainsKey(userId))
        {
            throw new InvalidOperationException($"User {userId} must be created before uploading biometrics.");
        }

        _log?.Invoke($"Simulated palm template upload for {userId} ({template.Data.Length} bytes)");
        return Task.CompletedTask;
    }

    public Task RemoveUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        if (_users.Remove(userId))
        {
            _transactions.Add(new DeviceTransaction(_transactions.Count + 1, userId, DateTimeOffset.UtcNow));
            _log?.Invoke($"Removed simulated user {userId}");
        }
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<DeviceTransaction>> ReadTransactionsAsync(long sincePointer, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        var items = _transactions
            .Where(t => t.Sequence > sincePointer)
            .OrderBy(t => t.Sequence)
            .ToArray();
        return Task.FromResult((IReadOnlyList<DeviceTransaction>)items);
    }

    public Task ControlRelayAsync(int relayPort, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        EnsureConnected();
        _log?.Invoke($"Simulated relay {relayPort} activated for {duration.TotalMilliseconds} ms");
        return Task.CompletedTask;
    }

    private void EnsureConnected()
    {
        if (!_connected)
        {
            throw new InvalidOperationException("Device must be connected before performing this operation.");
        }
    }
}
