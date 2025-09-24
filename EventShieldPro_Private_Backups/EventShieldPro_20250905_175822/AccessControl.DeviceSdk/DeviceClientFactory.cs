using AccessControl.DeviceSdk.Abstractions;
using AccessControl.DeviceSdk.Models;
using AccessControl.DeviceSdk.Native;
using AccessControl.DeviceSdk.Simulation;

namespace AccessControl.DeviceSdk;

public static class DeviceClientFactory
{
    public static IDeviceClient CreateSimulated(Action<string>? logHandler = null)
    {
        return new SimulatedDeviceClient(logHandler);
    }

    public static IDeviceClient CreateNative(DeviceConnectionOptions options)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return new DsF881DeviceClient(options);
    }
}
