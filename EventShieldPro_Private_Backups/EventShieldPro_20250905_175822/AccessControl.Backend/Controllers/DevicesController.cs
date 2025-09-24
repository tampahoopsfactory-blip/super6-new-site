using AccessControl.Backend.Device.Requests;
using AccessControl.Backend.Models;
using AccessControl.Backend.Services;
using AccessControl.DeviceSdk.Abstractions;
using AccessControl.DeviceSdk.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DevicesController : ControllerBase
{
    private readonly IDeviceClient _deviceClient;
    private readonly IDeviceCatalog _deviceCatalog;

    public DevicesController(IDeviceClient deviceClient, IDeviceCatalog deviceCatalog)
    {
        _deviceClient = deviceClient;
        _deviceCatalog = deviceCatalog;
    }

    [HttpGet]
    public IActionResult GetDevices()
    {
        var devices = _deviceCatalog.GetDevices();
        var payload = devices.Select(MapDeviceForResponse).ToList();

        return Ok(new
        {
            success = true,
            devices = payload,
            total = payload.Count
        });
    }

    [HttpPost("{deviceId:guid}/sync-time")]
    public async Task<IActionResult> SyncTimeAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var timestamp = DateTimeOffset.UtcNow;

        if (_deviceCatalog.GetDevice(deviceId) is null)
        {
            return NotFound(new { success = false, error = "Device not found" });
        }

        await _deviceClient.EnsureTimeSyncAsync(cancellationToken).ConfigureAwait(false);
        _deviceCatalog.UpdateLastSync(deviceId, timestamp);
        return Accepted(new { deviceId, syncedAtUtc = timestamp });
    }

    [HttpPost("{deviceId:guid}/relay")]
    public async Task<IActionResult> ControlRelayAsync(Guid deviceId, [FromBody] RelayRequest request, CancellationToken cancellationToken)
    {
        await _deviceClient.ControlRelayAsync(request.RelayPort, TimeSpan.FromMilliseconds(request.DurationMilliseconds), cancellationToken)
            .ConfigureAwait(false);
        return Accepted(new { deviceId, request.RelayPort, request.DurationMilliseconds });
    }

    [HttpPost("{deviceId:guid}/users")]
    public async Task<IActionResult> UpsertUserAsync(Guid deviceId, [FromBody] DeviceUserUpsertRequest request, CancellationToken cancellationToken)
    {
        var device = _deviceCatalog.GetDevice(deviceId);
        if (device is null)
        {
            return NotFound(new { success = false, error = "Device not found" });
        }

        var user = new DeviceUser
        {
            Id = request.UserId,
            DisplayName = request.DisplayName,
            PhoneNumber = request.Phone,
            TicketNumber = request.TicketNumber,
            EnablePalm = request.EnablePalm,
            EnableFace = request.EnableFace
        };

        await _deviceClient.UpsertUserAsync(user, cancellationToken).ConfigureAwait(false);

        if (request.FaceJpeg is { Length: > 0 })
        {
            await _deviceClient.UploadFaceAsync(user.Id, request.FaceJpeg, cancellationToken).ConfigureAwait(false);
        }

        if (request.PalmTemplate is { Length: > 0 })
        {
            var template = new BiometricTemplate(request.PalmTemplate, BiometricTemplateType.Palm);
            await _deviceClient.UploadPalmTemplateAsync(user.Id, template, cancellationToken).ConfigureAwait(false);
        }

        return Accepted(new { deviceId, user.Id });
    }

    private static object MapDeviceForResponse(DeviceResponse device)
    {
        return new
        {
            id = device.Id,
            name = device.Name,
            ip_address = device.IpAddress,
            port = device.Port,
            device_type = device.DeviceType,
            location = device.Location,
            is_online = device.IsOnline,
            status = device.Status,
            last_sync = device.LastSyncUtc
        };
    }
}
