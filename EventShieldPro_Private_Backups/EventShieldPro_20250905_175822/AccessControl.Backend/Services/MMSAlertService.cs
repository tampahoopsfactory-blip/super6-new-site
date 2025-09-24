using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AccessControl.Backend.Services;

public sealed class MMSAlertService
{
    private readonly ILogger<MMSAlertService> _logger;
    private readonly MMSOptions _options;

    public MMSAlertService(ILogger<MMSAlertService> logger, IOptions<MMSOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<bool> SendSecurityAlertAsync(SecurityAlertRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sending MMS security alert for {PersonName} - {Category}", 
                request.PersonName, request.CriminalCategory);

            // Compose the alert message
            var message = ComposeAlertMessage(request);

            // Send via carrier MMS gateway
            var success = await SendViaCarrierMMSAsync(request.PhoneNumber, message, request.FaceImageData, cancellationToken);
            
            if (success)
            {
                _logger.LogInformation("MMS security alert sent successfully to {PhoneNumber}", request.PhoneNumber);
            }
            else
            {
                _logger.LogWarning("Failed to send MMS security alert to {PhoneNumber}", request.PhoneNumber);
            }

            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending MMS security alert to {PhoneNumber}", request.PhoneNumber);
            return false;
        }
    }

    private string ComposeAlertMessage(SecurityAlertRequest request)
    {
        var timestamp = DateTimeOffset.Now.ToString("MM/dd/yyyy hh:mm tt");
        
        var message = new StringBuilder();
        message.AppendLine($"🚨 SECURITY ALERT - {request.CriminalCategory.ToUpperInvariant()} 🚨");
        message.AppendLine();
        message.AppendLine($"Guest: {request.PersonName}");
        message.AppendLine($"Ticket: {request.TicketNumber}");
        message.AppendLine($"Risk: {request.RiskLevel.ToUpperInvariant()}");
        message.AppendLine($"Gate: {request.Location}");
        message.AppendLine($"Time: {timestamp}");
        message.AppendLine();
        message.AppendLine("EventShield Pro Security System");

        return message.ToString();
    }

    private async Task<bool> SendViaCarrierMMSAsync(string phoneNumber, string message, string? faceImageData, CancellationToken cancellationToken)
    {
        try
        {
            // Clean phone number
            var cleanPhone = new string(phoneNumber.Where(char.IsDigit).ToArray());
            if (cleanPhone.Length == 11 && cleanPhone.StartsWith("1"))
                cleanPhone = cleanPhone[1..];
            
            if (cleanPhone.Length != 10)
            {
                _logger.LogError("Invalid phone number format: {PhoneNumber}", phoneNumber);
                return false;
            }

            // Get carrier MMS domain
            var domain = GetCarrierMMSDomain(phoneNumber);
            if (string.IsNullOrEmpty(domain))
            {
                _logger.LogError("Could not determine MMS domain for phone number: {PhoneNumber}", phoneNumber);
                return false;
            }

            var toEmail = $"{cleanPhone}@{domain}";

            // Create MMS message
            using var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_options.FromEmail, "EventShield Pro");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "EventShield Pro Security Alert";

            // Add text content
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = false;

            // Add face image if provided
            if (!string.IsNullOrEmpty(faceImageData))
            {
                try
                {
                    var imageData = faceImageData;
                    if (imageData.StartsWith("data:image"))
                    {
                        imageData = imageData.Split(',')[1];
                    }

                    var imageBytes = Convert.FromBase64String(imageData);
                    var imageStream = new MemoryStream(imageBytes);
                    var attachment = new Attachment(imageStream, "security_alert.jpg", "image/jpeg");
                    attachment.ContentId = "security_alert";
                    attachment.ContentDisposition!.Inline = true;
                    mailMessage.Attachments.Add(attachment);

                    _logger.LogInformation("Attached face image to MMS (bytes={ImageSize})", imageBytes.Length);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not attach face image to MMS");
                    return false;
                }
            }

            // Send via SMTP
            using var smtpClient = new SmtpClient(_options.SmtpServer, _options.SmtpPort);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(_options.Username, _options.Password);

            _logger.LogInformation("Sending MMS via SMTP to {ToEmail} using username {Username}", toEmail, _options.Username);
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
            
            _logger.LogInformation("MMS sent to {PhoneNumber} via {Domain}", phoneNumber, domain);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending MMS to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    private string GetCarrierMMSDomain(string phoneNumber)
    {
        // Clean phone number and get area code
        var cleanPhone = new string(phoneNumber.Where(char.IsDigit).ToArray());
        if (cleanPhone.Length == 11 && cleanPhone.StartsWith("1"))
            cleanPhone = cleanPhone[1..];

        if (cleanPhone.Length != 10)
            return "vzwpix.com"; // Default to Verizon

        var areaCode = cleanPhone[..3];

        // Area code to carrier mapping
        return areaCode switch
        {
            "813" or "727" or "941" => "vzwpix.com", // Tampa area - Verizon
            "305" or "786" or "954" => "tmomail.net", // Miami area - T-Mobile
            "212" or "646" or "718" => "vzwpix.com", // NYC area - Verizon
            _ => "vzwpix.com" // Default to Verizon
        };
    }
}

public sealed class SecurityAlertRequest
{
    public required string PhoneNumber { get; init; }
    public required string PersonName { get; init; }
    public required string CriminalCategory { get; init; }
    public required string RiskLevel { get; init; }
    public required string Location { get; init; }
    public required string TicketNumber { get; init; }
    public string? FaceImageData { get; init; }
    public Dictionary<string, object>? TicketInfo { get; init; }
}

public sealed class MMSOptions
{
    public string SmtpServer { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
}
