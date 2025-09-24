namespace AccessControl.Backend.Data.Entities;

public class DeviceEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public int Port { get; set; }
    public string DeviceType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset LastSyncUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }

    public ICollection<AccessLogEntity> AccessLogs { get; set; } = new List<AccessLogEntity>();
}

