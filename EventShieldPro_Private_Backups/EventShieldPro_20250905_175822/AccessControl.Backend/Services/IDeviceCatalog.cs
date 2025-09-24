using AccessControl.Backend.Models;

namespace AccessControl.Backend.Services;

public interface IDeviceCatalog
{
    IReadOnlyList<DeviceResponse> GetDevices();
    DeviceResponse? GetDevice(Guid id);
    bool UpdateLastSync(Guid id, DateTimeOffset timestamp);
}
