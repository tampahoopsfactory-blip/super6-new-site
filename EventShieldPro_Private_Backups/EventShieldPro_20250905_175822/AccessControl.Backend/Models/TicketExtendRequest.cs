using System.ComponentModel.DataAnnotations;

namespace AccessControl.Backend.Models;

public sealed record TicketExtendRequest(
    [property: Range(1, 168, ErrorMessage = "Extension hours must be between 1 and 168.")]
    int Hours
);
