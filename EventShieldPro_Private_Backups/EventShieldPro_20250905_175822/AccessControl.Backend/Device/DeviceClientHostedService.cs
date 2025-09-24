using AccessControl.DeviceSdk.Abstractions;
using Microsoft.Extensions.Hosting;

namespace AccessControl.Backend.Device;

public sealed class DeviceClientHostedService : IHostedService, IDisposable
{
    private readonly IDeviceClient _deviceClient;

    public DeviceClientHostedService(IDeviceClient deviceClient)
    {
        _deviceClient = deviceClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _deviceClient.ConnectAsync(cancellationToken).ConfigureAwait(false);
        await _deviceClient.EnsureTimeSyncAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public void Dispose()
    {
        _deviceClient.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}
