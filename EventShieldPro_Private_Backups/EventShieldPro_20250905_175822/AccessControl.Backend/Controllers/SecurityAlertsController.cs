using AccessControl.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SecurityAlertsController : ControllerBase
{
    private readonly MMSAlertService _mmsAlertService;
    private readonly ILogger<SecurityAlertsController> _logger;

    public SecurityAlertsController(MMSAlertService mmsAlertService, ILogger<SecurityAlertsController> logger)
    {
        _mmsAlertService = mmsAlertService;
        _logger = logger;
    }

    [HttpPost("criminal-alert")]
    [ProducesResponseType(typeof(SecurityAlertResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendCriminalAlertAsync([FromBody] SecurityAlertRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.PhoneNumber) ||
                string.IsNullOrWhiteSpace(request.PersonName) ||
                string.IsNullOrWhiteSpace(request.CriminalCategory) ||
                string.IsNullOrWhiteSpace(request.RiskLevel) ||
                string.IsNullOrWhiteSpace(request.TicketNumber))
            {
                return BadRequest(new { error = "Missing required fields: PhoneNumber, PersonName, CriminalCategory, RiskLevel, TicketNumber" });
            }

            _logger.LogInformation("Processing criminal alert for {PersonName} - {Category}", 
                request.PersonName, request.CriminalCategory);

            // Send MMS alert
            var success = await _mmsAlertService.SendSecurityAlertAsync(request, cancellationToken);

            var response = new SecurityAlertResponse
            {
                Success = success,
                AlertId = Guid.NewGuid().ToString(),
                PersonName = request.PersonName,
                CriminalCategory = request.CriminalCategory,
                RiskLevel = request.RiskLevel,
                FaceImageIncluded = !string.IsNullOrEmpty(request.FaceImageData),
                TicketInfoIncluded = request.TicketInfo != null && request.TicketInfo.Count > 0,
                MessageType = !string.IsNullOrEmpty(request.FaceImageData) ? "MMS" : "SMS",
                Timestamp = DateTimeOffset.UtcNow
            };

            if (success)
            {
                _logger.LogInformation("Criminal alert sent successfully for {PersonName}", request.PersonName);
                return Ok(response);
            }
            else
            {
                _logger.LogWarning("Failed to send criminal alert for {PersonName}", request.PersonName);
                response.ErrorMessage = "Failed to send MMS alert";
                return Ok(response); // Return 200 with success=false for client handling
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing criminal alert for {PersonName}", request.PersonName);
            return StatusCode(500, new { error = "Internal server error processing criminal alert" });
        }
    }

    [HttpPost("test")]
    [ProducesResponseType(typeof(SecurityAlertResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> TestMMSAsync([FromBody] TestMMSRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var testAlert = new SecurityAlertRequest
            {
                PhoneNumber = request.PhoneNumber,
                PersonName = "Test User",
                CriminalCategory = "Test",
                RiskLevel = "Low",
                Location = "Test Location",
                TicketNumber = "TEST-001",
                FaceImageData = request.IncludeImage ? CreateTestImage() : null,
                TicketInfo = new Dictionary<string, object>
                {
                    ["test"] = true,
                    ["timestamp"] = DateTimeOffset.UtcNow
                }
            };

            var success = await _mmsAlertService.SendSecurityAlertAsync(testAlert, cancellationToken);

            var response = new SecurityAlertResponse
            {
                Success = success,
                AlertId = "TEST-" + Guid.NewGuid().ToString("N")[..8],
                PersonName = testAlert.PersonName,
                CriminalCategory = testAlert.CriminalCategory,
                RiskLevel = testAlert.RiskLevel,
                FaceImageIncluded = !string.IsNullOrEmpty(testAlert.FaceImageData),
                TicketInfoIncluded = true,
                MessageType = request.IncludeImage ? "MMS" : "SMS",
                Timestamp = DateTimeOffset.UtcNow
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error testing MMS for {PhoneNumber}", request.PhoneNumber);
            return StatusCode(500, new { error = "Internal server error testing MMS" });
        }
    }

    private static string CreateTestImage()
    {
        // Simple 1x1 pixel JPEG image in base64 (without data URL prefix)
        return "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAABAAEDASIAAhEBAxEB/8QAFQABAQAAAAAAAAAAAAAAAAAAAAv/xAAUEAEAAAAAAAAAAAAAAAAAAAAA/8QAFQEBAQAAAAAAAAAAAAAAAAAAAAX/xAAUEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwA/8A==";
    }
}

public sealed class SecurityAlertResponse
{
    public bool Success { get; set; }
    public string AlertId { get; set; } = string.Empty;
    public string PersonName { get; set; } = string.Empty;
    public string CriminalCategory { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public bool FaceImageIncluded { get; set; }
    public bool TicketInfoIncluded { get; set; }
    public string MessageType { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}

public sealed class TestMMSRequest
{
    public required string PhoneNumber { get; init; }
    public bool IncludeImage { get; init; } = true;
}
