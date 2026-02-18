using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace RideFlow.Models;

public partial class RideflowContext : DbContext
{
    public RideflowContext(DbContextOptions<RideflowContext> options) : base(options) { }

    public virtual DbSet<TbDriver> TbDrivers { get; set; }
    public virtual DbSet<TbRating> TbRatings { get; set; }
    public virtual DbSet<TbRide> TbRides { get; set; }
    public virtual DbSet<TbServicetype> TbServicetypes { get; set; }
    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Conversores para os enums (serão armazenados como string no banco)
        var rideStatusConverter = new ValueConverter<RideStatus, string>(
            v => v.ToString(),
            v => (RideStatus)Enum.Parse(typeof(RideStatus), v));

        var paymentMethodConverter = new ValueConverter<PaymentMethod, string>(
            v => v.ToString(),
            v => (PaymentMethod)Enum.Parse(typeof(PaymentMethod), v));

        var serviceCategoryConverter = new ValueConverter<ServiceCategory, string>(
            v => v.ToString(),
            v => (ServiceCategory)Enum.Parse(typeof(ServiceCategory), v));

        // --- tb_driver (igual antes) ---
        modelBuilder.Entity<TbDriver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_driver_pkey");
            entity.ToTable("tb_driver");
            entity.HasIndex(e => e.Cnh, "tb_driver_cnh_key").IsUnique();
            entity.HasIndex(e => e.Plate, "tb_driver_plate_key").IsUnique();
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Namedriver).HasMaxLength(256).HasColumnName("namedriver");
            entity.Property(e => e.Cnh).HasMaxLength(9).HasColumnName("cnh");
            entity.Property(e => e.Plate).HasMaxLength(7).HasColumnName("plate");
            entity.Property(e => e.Yearcar).HasColumnName("yearcar");
            entity.Property(e => e.Modelcar).HasMaxLength(256).HasColumnName("modelcar");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()").HasColumnName("created_at");

            entity.HasMany(d => d.Servicetypes)
                .WithMany(p => p.Drivers)
                .UsingEntity<Dictionary<string, object>>(
                    "driver_servicetype",
                    r => r.HasOne<TbServicetype>().WithMany().HasForeignKey("servicetype_id"),
                    l => l.HasOne<TbDriver>().WithMany().HasForeignKey("driver_id"),
                    j =>
                    {
                        j.HasKey("driver_id", "servicetype_id").HasName("driver_servicetype_pkey");
                        j.ToTable("driver_servicetype");
                    });
        });

        // --- tb_user (igual antes) ---
        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_user_pkey");
            entity.ToTable("tb_user");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Nameuser).HasMaxLength(256).HasColumnName("nameuser");
            entity.Property(e => e.Cpf).HasMaxLength(11).HasColumnName("cpf");
            entity.Property(e => e.Phoneuser).HasMaxLength(13).HasColumnName("phoneuser");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()").HasColumnName("created_at");
        });

        // --- tb_ride com conversão para string ---
        modelBuilder.Entity<TbRide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_ride_pkey");
            entity.ToTable("tb_ride");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Startpoint).HasMaxLength(256).HasColumnName("startpoint");
            entity.Property(e => e.Destiny).HasMaxLength(256).HasColumnName("destiny");

            entity.Property(e => e.Status)
                .HasColumnName("ride_status")
                .HasConversion(rideStatusConverter)  // Converte enum <-> string
                .HasMaxLength(50);                   // Opcional, mas compatível com varchar(50)

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.ServicetypeId).HasColumnName("servicetype_id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()").HasColumnName("created_at");

            entity.Property(e => e.DistanceKm).HasColumnName("distance_km");
            entity.Property(e => e.TotalValue).HasColumnName("total_value");

            entity.Property(e => e.PaymentMethod)
                .HasColumnName("payment_method")
                .HasConversion(paymentMethodConverter)  // Converte enum <-> string
                .HasMaxLength(50);

            entity.HasOne(d => d.Driver)
                .WithMany(p => p.TbRides)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ride_driver");

            entity.HasOne(d => d.Servicetype)
                .WithMany(p => p.TbRides)
                .HasForeignKey(d => d.ServicetypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ride_serviceType");

            entity.HasOne(d => d.User)
                .WithMany(p => p.TbRides)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ride_user");
        });

        // --- tb_servicetype com conversão para string ---
        modelBuilder.Entity<TbServicetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_servicetype_pkey");
            entity.ToTable("tb_servicetype");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");

            entity.Property(e => e.Category)
                .HasColumnName("category")
                .HasConversion(serviceCategoryConverter)  // Converte enum <-> string
                .HasMaxLength(50);
        });

        // --- tb_rating (igual antes) ---
        modelBuilder.Entity<TbRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_rating_pkey");
            entity.ToTable("tb_rating");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()").HasColumnName("id");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Comment).HasMaxLength(256).HasColumnName("comment");
            entity.Property(e => e.RideId).HasColumnName("ride_id");
            entity.HasOne(d => d.Ride).WithMany(p => p.TbRatings).HasForeignKey(d => d.RideId).HasConstraintName("fk_rating_ride");
        });
    }
}