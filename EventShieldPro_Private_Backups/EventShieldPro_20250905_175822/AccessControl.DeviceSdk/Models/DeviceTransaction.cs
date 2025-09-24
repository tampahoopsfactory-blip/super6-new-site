namespace AccessControl.DeviceSdk.Models;

public sealed class DeviceTransaction
{
    public DeviceTransaction(long sequence, Guid userId, DateTimeOffset occurredAtUtc, string? photoUrl = null)
    {
        Sequence = sequence;
        UserId = userId;
        OccurredAtUtc = occurredAtUtc;
        PhotoUrl = photoUrl;
    }

    public long Sequence { get; }
    public Guid UserId { get; }
    public DateTimeOffset OccurredAtUtc { get; }
    public string? PhotoUrl { get; }
}
