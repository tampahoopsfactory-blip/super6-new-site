using AccessControl.Backend.Data;
using AccessControl.Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccessControl.Backend.Services;

public sealed class EfDeviceCatalog : IDeviceCatalog
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<EfDeviceCatalog> _logger;

    public EfDeviceCatalog(ApplicationDbContext dbContext, ILogger<EfDeviceCatalog> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IReadOnlyList<DeviceResponse> GetDevices()
    {
        var devices = _dbContext.Devices
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .Select(d => new DeviceResponse(
                d.Id,
                d.Name,
                d.IpAddress,
                d.Port,
                d.DeviceType,
                d.Location,
                d.IsOnline,
                d.Status,
                d.LastSyncUtc))
            .ToList();

        return devices;
    }

    public DeviceResponse? GetDevice(Guid id)
    {
        return _dbContext.Devices
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DeviceResponse(
                d.Id,
                d.Name,
                d.IpAddress,
                d.Port,
                d.DeviceType,
                d.Location,
                d.IsOnline,
                d.Status,
                d.LastSyncUtc))
            .FirstOrDefault();
    }

    public bool UpdateLastSync(Guid id, DateTimeOffset timestamp)
    {
        var device = _dbContext.Devices.FirstOrDefault(d => d.Id == id);
        if (device is null)
        {
            _logger.LogWarning("Attempted to update last sync for unknown device {DeviceId}", id);
            return false;
        }

        device.LastSyncUtc = timestamp;
        device.UpdatedAtUtc = DateTimeOffset.UtcNow;
        device.IsOnline = true;
        device.Status = "Operational";

        _dbContext.SaveChanges();
        return true;
    }
}
