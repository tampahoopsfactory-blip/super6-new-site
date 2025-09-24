using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AccessControl.Backend.Data.Entities;
using AccessControl.Domain.Tickets;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Backend.Data;

public static class ApplicationDbSeeder
{
    private static readonly Dictionary<string, int> TicketSequences = new();

    private static string NormalizeVenueCode(string venueCode)
    {
        if (string.IsNullOrWhiteSpace(venueCode))
        {
            return "UN";
        }

        var letters = new string(venueCode.Where(char.IsLetter).ToArray()).ToUpperInvariant();

        if (letters.Length >= 2)
        {
            return letters[..2];
        }

        if (letters.Length == 1)
        {
            return letters + "X";
        }

        return "UN";
    }

    private static string FormatTicketNumber(string venueCode, DateTimeOffset purchasedAt)
    {
        var normalizedVenue = NormalizeVenueCode(venueCode);
        var utcTime = purchasedAt.ToOffset(TimeSpan.Zero);
        var dateKey = utcTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        var sequenceKey = $"{normalizedVenue}-{dateKey}";
        var next = TicketSequences.TryGetValue(sequenceKey, out var current) ? current + 1 : 1;
        if (next > 99999)
        {
            next = 1;
        }

        TicketSequences[sequenceKey] = next;

        var dateSegment = utcTime.ToString("MMddyy", CultureInfo.InvariantCulture);
        return $"{normalizedVenue}-{dateSegment}-{next:00000}";
    }

    public static async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Guests.AsNoTracking().AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;

        var guestAlice = new GuestEntity
        {
            Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
            Name = "Alice Johnson",
            Phone = "+1-813-555-0100",
            Email = "alice@example.com",
            FacePhotoUrl = null,
            CreatedAtUtc = now.AddDays(-3),
            UpdatedAtUtc = now.AddHours(-6)
        };

        var guestBrian = new GuestEntity
        {
            Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
            Name = "Brian Martinez",
            Phone = "+1-813-555-0142",
            Email = "brian@example.com",
            FacePhotoUrl = null,
            CreatedAtUtc = now.AddDays(-1),
            UpdatedAtUtc = now.AddHours(-1)
        };

        var ticketAlice = new TicketEntity
        {
            Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"),
            Number = FormatTicketNumber("MAIN", now.AddDays(-1)),
            Guest = guestAlice,
            GuestId = guestAlice.Id,
            Type = TicketType.Daily,
            Status = TicketStatus.Active,
            PurchasedAt = now.AddDays(-1),
            ExpiresAt = now.AddHours(10),
            VenueCode = "MAIN"
        };

        var ticketBrian = new TicketEntity
        {
            Id = Guid.Parse("d4444444-4444-4444-4444-444444444444"),
            Number = FormatTicketNumber("VIP", now.AddDays(-2)),
            Guest = guestBrian,
            GuestId = guestBrian.Id,
            Type = TicketType.Weekend,
            Status = TicketStatus.Active,
            PurchasedAt = now.AddDays(-2),
            ExpiresAt = now.AddDays(1),
            VenueCode = "VIP"
        };

        var ticketExpired = new TicketEntity
        {
            Id = Guid.Parse("e5555555-5555-5555-5555-555555555555"),
            Number = FormatTicketNumber("ANNEX", now.AddDays(-5)),
            Guest = guestBrian,
            GuestId = guestBrian.Id,
            Type = TicketType.Daily,
            Status = TicketStatus.Expired,
            PurchasedAt = now.AddDays(-5),
            ExpiresAt = now.AddDays(-4).AddHours(-22),
            VenueCode = "ANNEX"
        };

        var deviceMain = new DeviceEntity
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Main Gate DS-F881",
            IpAddress = "192.168.1.10",
            Port = 9000,
            DeviceType = "DS-F881 Facial Recognition Terminal",
            Location = "Main Entrance",
            IsOnline = true,
            Status = "Operational",
            LastSyncUtc = now.AddMinutes(-2),
            CreatedAtUtc = now.AddMonths(-1),
            UpdatedAtUtc = now.AddMinutes(-2)
        };

        var deviceVip = new DeviceEntity
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "VIP Lounge DS-F881",
            IpAddress = "192.168.1.20",
            Port = 9000,
            DeviceType = "DS-F881 Facial Recognition Terminal",
            Location = "VIP Lounge",
            IsOnline = true,
            Status = "Operational",
            LastSyncUtc = now.AddMinutes(-5),
            CreatedAtUtc = now.AddMonths(-1),
            UpdatedAtUtc = now.AddMinutes(-5)
        };

        var deviceWest = new DeviceEntity
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Name = "Turnstile West DSN-50P",
            IpAddress = "192.168.1.30",
            Port = 9100,
            DeviceType = "DSN-50P Turnstile Controller",
            Location = "West Gate",
            IsOnline = false,
            Status = "Offline",
            LastSyncUtc = now.AddHours(-3),
            CreatedAtUtc = now.AddMonths(-1),
            UpdatedAtUtc = now.AddHours(-3)
        };

        var logs = new List<AccessLogEntity>
        {
            new()
            {
                Id = Guid.Parse("f6666666-6666-6666-6666-666666666666"),
                Device = deviceMain,
                DeviceId = deviceMain.Id,
                Guest = guestAlice,
                GuestId = guestAlice.Id,
                Result = "granted",
                Details = "Ticket RM entry granted",
                Timestamp = now.AddMinutes(-7)
            },
            new()
            {
                Id = Guid.Parse("f7777777-7777-7777-7777-777777777777"),
                Device = deviceVip,
                DeviceId = deviceVip.Id,
                Guest = guestBrian,
                GuestId = guestBrian.Id,
                Result = "denied",
                Details = "Expired ticket presented",
                Timestamp = now.AddHours(-3)
            },
            new()
            {
                Id = Guid.Parse("f8888888-8888-8888-8888-888888888888"),
                Device = deviceWest,
                DeviceId = deviceWest.Id,
                Guest = null,
                GuestId = null,
                Result = "alert",
                Details = "Unregistered face detected",
                Timestamp = now.AddHours(-1)
            }
        };

        await dbContext.Guests.AddRangeAsync(new[] { guestAlice, guestBrian }, cancellationToken).ConfigureAwait(false);
        await dbContext.Tickets.AddRangeAsync(new[] { ticketAlice, ticketBrian, ticketExpired }, cancellationToken).ConfigureAwait(false);
        await dbContext.Devices.AddRangeAsync(new[] { deviceMain, deviceVip, deviceWest }, cancellationToken).ConfigureAwait(false);
        await dbContext.AccessLogs.AddRangeAsync(logs, cancellationToken).ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
