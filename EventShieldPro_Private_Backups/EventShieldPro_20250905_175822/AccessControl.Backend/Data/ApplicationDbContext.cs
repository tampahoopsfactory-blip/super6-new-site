using AccessControl.Backend.Data.Entities;
using AccessControl.Domain.Tickets;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Backend.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<GuestEntity> Guests => Set<GuestEntity>();
    public DbSet<TicketEntity> Tickets => Set<TicketEntity>();
    public DbSet<DeviceEntity> Devices => Set<DeviceEntity>();
    public DbSet<AccessLogEntity> AccessLogs => Set<AccessLogEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureGuests(modelBuilder);
        ConfigureTickets(modelBuilder);
        ConfigureDevices(modelBuilder);
        ConfigureAccessLogs(modelBuilder);
    }

    private static void ConfigureGuests(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<GuestEntity>();
        entity.ToTable("Guests");
        entity.HasKey(g => g.Id);
        entity.Property(g => g.Name).IsRequired().HasMaxLength(160);
        entity.Property(g => g.Phone).IsRequired().HasMaxLength(32);
        entity.Property(g => g.Email).HasMaxLength(160);
        entity.Property(g => g.FacePhotoUrl).HasMaxLength(512);
        entity.Property(g => g.CreatedAtUtc).IsRequired();
        entity.Property(g => g.UpdatedAtUtc).IsRequired();
        entity.HasIndex(g => g.Phone).IsUnique();
    }

    private static void ConfigureTickets(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<TicketEntity>();
        entity.ToTable("Tickets");
        entity.HasKey(t => t.Id);
        entity.Property(t => t.Number).IsRequired().HasMaxLength(64);
        entity.Property(t => t.Type).HasConversion<string>().HasMaxLength(32).IsRequired();
        entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        entity.Property(t => t.VenueCode).IsRequired().HasMaxLength(32);
        entity.Property(t => t.PurchasedAt).IsRequired();
        entity.Property(t => t.ExpiresAt).IsRequired();
        entity.Property(t => t.GroupId);
        entity.Property(t => t.FamilyMemberIndex);
        entity.Property(t => t.PhoneExtension).HasMaxLength(8);
        entity.HasIndex(t => new { t.Status, t.ExpiresAt });
        entity.HasIndex(t => t.Number).IsUnique();

        entity.HasOne(t => t.Guest)
            .WithMany(g => g.Tickets)
            .HasForeignKey(t => t.GuestId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureDevices(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<DeviceEntity>();
        entity.ToTable("Devices");
        entity.HasKey(d => d.Id);
        entity.Property(d => d.Name).IsRequired().HasMaxLength(160);
        entity.Property(d => d.IpAddress).IsRequired().HasMaxLength(64);
        entity.Property(d => d.Port).IsRequired();
        entity.Property(d => d.DeviceType).IsRequired().HasMaxLength(64);
        entity.Property(d => d.Location).HasMaxLength(160);
        entity.Property(d => d.Status).IsRequired().HasMaxLength(32);
        entity.Property(d => d.LastSyncUtc).IsRequired();
        entity.Property(d => d.CreatedAtUtc).IsRequired();
        entity.Property(d => d.UpdatedAtUtc).IsRequired();
    }

    private static void ConfigureAccessLogs(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<AccessLogEntity>();
        entity.ToTable("AccessLogs");
        entity.HasKey(l => l.Id);
        entity.Property(l => l.Result).IsRequired().HasMaxLength(64);
        entity.Property(l => l.Details).HasMaxLength(512);
        entity.Property(l => l.Timestamp).IsRequired();

        entity.HasOne(l => l.Device)
            .WithMany(d => d.AccessLogs)
            .HasForeignKey(l => l.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(l => l.Guest)
            .WithMany()
            .HasForeignKey(l => l.GuestId)
            .OnDelete(DeleteBehavior.SetNull);

        entity.HasIndex(l => l.Timestamp);
    }
}

