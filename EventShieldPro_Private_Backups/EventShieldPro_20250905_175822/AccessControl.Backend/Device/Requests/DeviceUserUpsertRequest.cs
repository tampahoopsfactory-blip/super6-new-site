using System.ComponentModel.DataAnnotations;

namespace AccessControl.Backend.Device.Requests;

public sealed record DeviceUserUpsertRequest(
    [property: Required] Guid UserId,
    [property: Required] string DisplayName,
    [property: Required] string TicketNumber,
    [property: Required] string Phone,
    bool EnablePalm,
    bool EnableFace,
    byte[]? FaceJpeg,
    byte[]? PalmTemplate
);
