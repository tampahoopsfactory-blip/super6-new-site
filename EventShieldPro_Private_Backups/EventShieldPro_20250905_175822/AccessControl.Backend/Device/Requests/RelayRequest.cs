using System.ComponentModel.DataAnnotations;

namespace AccessControl.Backend.Device.Requests;

public sealed record RelayRequest(
    [property: Range(0, 8)] int RelayPort,
    [property: Range(100, 10000)] int DurationMilliseconds
);
