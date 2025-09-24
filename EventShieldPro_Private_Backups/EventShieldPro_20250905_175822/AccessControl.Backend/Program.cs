using AccessControl.Backend.Data;
using System;
using AccessControl.Backend.Device;
using AccessControl.Backend.Repositories;
using AccessControl.Backend.Services;
using AccessControl.DeviceSdk;
using AccessControl.DeviceSdk.Abstractions;
using AccessControl.DeviceSdk.Models;
using AccessControl.Domain.Tickets;
using AccessControl.Domain.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5001");

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=eventshield_backend.db";
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddSingleton<ITicketExpiryCalculator, TicketExpiryCalculator>();
builder.Services.AddSingleton<TicketFactory>();
builder.Services.AddScoped<ITicketRepository, EfTicketRepository>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<IDeviceCatalog, EfDeviceCatalog>();

// MMS Alert Service
builder.Services.Configure<MMSOptions>(options =>
{
    builder.Configuration.GetSection("MMS").Bind(options);

    options.SmtpServer = Environment.GetEnvironmentVariable("EVENTSHIELD_MMS_SMTP_SERVER") ?? options.SmtpServer;
    options.SmtpPort = int.TryParse(Environment.GetEnvironmentVariable("EVENTSHIELD_MMS_SMTP_PORT"), out var portOverride)
        ? portOverride
        : options.SmtpPort;
    options.Username = Environment.GetEnvironmentVariable("EVENTSHIELD_MMS_SMTP_USERNAME") ?? options.Username;
    options.Password = Environment.GetEnvironmentVariable("EVENTSHIELD_MMS_SMTP_PASSWORD") ?? options.Password;
    options.FromEmail = Environment.GetEnvironmentVariable("EVENTSHIELD_MMS_SMTP_FROM") ?? options.FromEmail;
});
builder.Services.AddSingleton<MMSAlertService>();

builder.Services.AddOptions<DeviceConnectionOptions>()
    .Bind(builder.Configuration.GetSection("DeviceConnection"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.Host), "DeviceConnection:Host must be provided")
    .Validate(o => !string.IsNullOrWhiteSpace(o.SerialNumber), "DeviceConnection:SerialNumber must be provided")
    .Validate(o => !string.IsNullOrWhiteSpace(o.CommunicationPassword), "DeviceConnection:CommunicationPassword must be provided")
    .ValidateOnStart();

builder.Services.AddSingleton<IDeviceClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<DeviceConnectionOptions>>().Value;
    return DeviceClientFactory.CreateNative(options);
});
builder.Services.AddHostedService<DeviceClientHostedService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await ApplicationDbSeeder.SeedAsync(dbContext);
}

app.UseCors();

app.MapControllers();

await app.RunAsync();
