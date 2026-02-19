using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RideFlow.Models;

[Table("tb_ride")]
public partial class TbRide
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("startpoint")]
    public string Startpoint { get; set; } = null!;

    [Column("destiny")]
    public string Destiny { get; set; } = null!;

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("driver_id")]
    public Guid DriverId { get; set; }

    [Column("ride_status")]
    public RideStatus Status { get; set; }

    [Column("servicetype_id")]
    public Guid ServicetypeId { get; set; }

    [Column("distance_km")]
    public decimal DistanceKm { get; set; }

    [Column("total_value")]
    public decimal TotalValue { get; set; }

    [Column("payment_method")]
    public PaymentMethod PaymentMethod { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    public virtual TbDriver Driver { get; set; } = null!;
    public virtual TbServicetype Servicetype { get; set; } = null!;
    public virtual TbUser User { get; set; } = null!;
    public virtual ICollection<TbRating> TbRatings { get; set; } = new List<TbRating>();
}