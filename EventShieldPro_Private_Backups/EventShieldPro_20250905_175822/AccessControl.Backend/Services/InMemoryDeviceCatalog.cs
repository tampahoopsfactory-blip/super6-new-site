using System.Collections.Concurrent;
using AccessControl.Backend.Models;

namespace AccessControl.Backend.Services;

public sealed class InMemoryDeviceCatalog : IDeviceCatalog
{
    private readonly ConcurrentDictionary<Guid, DeviceState> _devices;

    public InMemoryDeviceCatalog()
    {
        var now = DateTimeOffset.UtcNow;
        var seeds = new[]
        {
            new DeviceState(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Main Gate DS-F881",
                "192.168.1.10",
                9000,
                "DS-F881 Facial Recognition Terminal",
                "Main Entrance",
                true,
                "Operational",
                now),
            new DeviceState(
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "VIP Lounge DS-F881",
                "192.168.1.20",
                9000,
                "DS-F881 Facial Recognition Terminal",
                "VIP Lounge",
                true,
                "Operational",
                now.AddMinutes(-5)),
            new DeviceState(
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                "Turnstile West DSN-50P",
                "192.168.1.30",
                9100,
                "DSN-50P Turnstile Controller",
                "West Gate",
                false,
                "Offline",
                now.AddHours(-2))
        };

        _devices = new ConcurrentDictionary<Guid, DeviceState>(
            seeds.ToDictionary(device => device.Id));
    }

    public IReadOnlyList<DeviceResponse> GetDevices()
    {
        return _devices.Values
            .Select(device => device.ToResponse())
            .OrderBy(device => device.Name)
            .ToList();
    }

    public DeviceResponse? GetDevice(Guid id)
    {
        return _devices.TryGetValue(id, out var device)
            ? device.ToResponse()
            : null;
    }

    public bool UpdateLastSync(Guid id, DateTimeOffset timestamp)
    {
        if (!_devices.TryGetValue(id, out var device))
        {
            return false;
        }

        device.UpdateLastSync(timestamp);
        return true;
    }

    private sealed class DeviceState
    {
        private readonly object _syncRoot = new();

        public Guid Id { get; }
        public string Name { get; }
        public string IpAddress { get; }
        public int Port { get; }
        public string DeviceType { get; }
        public string Location { get; }
        public bool IsOnline { get; private set; }
        public string Status { get; private set; }
        public DateTimeOffset LastSyncUtc { get; private set; }

        public DeviceState(
            Guid id,
            string name,
            string ipAddress,
            int port,
            string deviceType,
            string location,
            bool isOnline,
            string status,
            DateTimeOffset lastSyncUtc)
        {
            Id = id;
            Name = name;
            IpAddress = ipAddress;
            Port = port;
            DeviceType = deviceType;
            Location = location;
            IsOnline = isOnline;
            Status = status;
            LastSyncUtc = lastSyncUtc;
        }

        public void UpdateLastSync(DateTimeOffset timestamp)
        {
            lock (_syncRoot)
            {
                LastSyncUtc = timestamp;
                IsOnline = true;
                Status = "Operational";
            }
        }

        public DeviceResponse ToResponse()
        {
            lock (_syncRoot)
            {
                return new DeviceResponse(
                    Id,
                    Name,
                    IpAddress,
                    Port,
                    DeviceType,
                    Location,
                    IsOnline,
                    Status,
                    LastSyncUtc);
            }
        }
    }
}
