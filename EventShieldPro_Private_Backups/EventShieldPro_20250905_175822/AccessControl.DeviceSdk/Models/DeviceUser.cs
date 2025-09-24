namespace AccessControl.DeviceSdk.Models;

public sealed class DeviceUser
{
    public Guid Id { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string TicketNumber { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public bool EnablePalm { get; init; }
    public bool EnableFace { get; init; } = true;
}
