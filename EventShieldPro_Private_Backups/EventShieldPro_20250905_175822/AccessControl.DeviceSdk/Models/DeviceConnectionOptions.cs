namespace AccessControl.DeviceSdk.Models;

public sealed class DeviceConnectionOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 8000;
    public int UdpPort { get; set; } = 8101;
    public string SerialNumber { get; set; } = string.Empty;
    public string CommunicationPassword { get; set; } = string.Empty;
    public string? LocalIpAddress { get; set; }
    public int RetryCount { get; set; } = 3;
    public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(5);
}
