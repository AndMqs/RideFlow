using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RideFlow.Models;

public partial class RideflowContext : DbContext
{
    public RideflowContext()
    {
    }

    public RideflowContext(DbContextOptions<RideflowContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbDriver> TbDrivers { get; set; }

    public virtual DbSet<TbRating> TbRatings { get; set; }

    public virtual DbSet<TbRide> TbRides { get; set; }

    public virtual DbSet<TbServicetype> TbServicetypes { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=rideflow;Username=postgres;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("payment_method", new[] { "pix", "credit_card", "debit_card" })
            .HasPostgresEnum("ride_status", new[] { "in_progress", "finished", "canceled" })
            .HasPostgresEnum("service_category", new[] { "basic", "premium", "vip" });

        modelBuilder.Entity<TbDriver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_driver_pkey");

            entity.ToTable("tb_driver");

            entity.HasIndex(e => e.Cnh, "tb_driver_cnh_key").IsUnique();

            entity.HasIndex(e => e.Plate, "tb_driver_plate_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cnh)
                .HasMaxLength(9)
                .HasColumnName("cnh");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Modelcar)
                .HasMaxLength(256)
                .HasColumnName("modelcar");
            entity.Property(e => e.Namedriver)
                .HasMaxLength(256)
                .HasColumnName("namedriver");
            entity.Property(e => e.Plate)
                .HasMaxLength(7)
                .HasColumnName("plate");
            entity.Property(e => e.Yearcar).HasColumnName("yearcar");
    

            entity.HasMany(d => d.Servicetypes).WithMany(p => p.Drivers)
                .UsingEntity<Dictionary<string, object>>(
                    "DriverServicetype",
                    r => r.HasOne<TbServicetype>().WithMany()
                        .HasForeignKey("ServicetypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_driver_servicetype"),
                    l => l.HasOne<TbDriver>().WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_driver"),
                    j =>
                    {
                        j.HasKey("DriverId", "ServicetypeId").HasName("driver_servicetype_pkey");
                        j.ToTable("driver_servicetype");
                        j.IndexerProperty<Guid>("DriverId").HasColumnName("driver_id");
                        j.IndexerProperty<Guid>("ServicetypeId").HasColumnName("servicetype_id");
                    });
        });

        modelBuilder.Entity<TbRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_rating_pkey");

            entity.ToTable("tb_rating");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Comment)
                .HasMaxLength(256)
                .HasColumnName("comment");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.RideId).HasColumnName("ride_id");

            entity.HasOne(d => d.Ride).WithMany(p => p.TbRatings)
                .HasForeignKey(d => d.RideId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rating_ride");
        });

        modelBuilder.Entity<TbRide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_ride_pkey");

            entity.ToTable("tb_ride");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Destiny)
                .HasMaxLength(256)
                .HasColumnName("destiny");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.ServicetypeId).HasColumnName("servicetype_id");
            entity.Property(e => e.Startpoint)
                .HasMaxLength(256)
                .HasColumnName("startpoint");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("ride_status");

            entity.HasOne(d => d.Driver).WithMany(p => p.TbRides)
                .HasForeignKey(d => d.DriverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ride_driver");

            entity.HasOne(d => d.Servicetype).WithMany(p => p.TbRides)
                .HasForeignKey(d => d.ServicetypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ride_servicetype");

            entity.HasOne(d => d.User).WithMany(p => p.TbRides)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ride_user");
        });

        modelBuilder.Entity<TbServicetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_servicetype_pkey");

            entity.ToTable("tb_servicetype");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");

            entity.Property(e => e.Category)
                .HasColumnName("category")
                .HasColumnType("service_category");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tb_user_pkey");

            entity.ToTable("tb_user");

            entity.HasIndex(e => e.Cpf, "tb_user_cpf_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .HasColumnName("cpf");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Nameuser)
                .HasMaxLength(256)
                .HasColumnName("nameuser");
            entity.Property(e => e.Phoneuser)
                .HasMaxLength(13)
                .HasColumnName("phoneuser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
