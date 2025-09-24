using AccessControl.DeviceSdk.Models;

namespace AccessControl.DeviceSdk.Abstractions;

public interface IDeviceClient : IAsyncDisposable
{
    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task EnsureTimeSyncAsync(CancellationToken cancellationToken = default);
    Task UpsertUserAsync(DeviceUser user, CancellationToken cancellationToken = default);
    Task UploadFaceAsync(Guid userId, byte[] jpegBytes, CancellationToken cancellationToken = default);
    Task UploadPalmTemplateAsync(Guid userId, BiometricTemplate template, CancellationToken cancellationToken = default);
    Task RemoveUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeviceTransaction>> ReadTransactionsAsync(long sincePointer, CancellationToken cancellationToken = default);
    Task ControlRelayAsync(int relayPort, TimeSpan duration, CancellationToken cancellationToken = default);
}
