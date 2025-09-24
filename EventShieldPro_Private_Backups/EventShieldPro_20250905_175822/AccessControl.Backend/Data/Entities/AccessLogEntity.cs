namespace AccessControl.Backend.Data.Entities;

public class AccessLogEntity
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public Guid? GuestId { get; set; }
    public string Result { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public DeviceEntity Device { get; set; } = null!;
    public GuestEntity? Guest { get; set; }
}

