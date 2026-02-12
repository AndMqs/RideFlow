using System;
using System.Collections.Generic;

namespace RideFlow.Models;

public partial class TbRide
{
    public Guid Id { get; set; }

    public string Startpoint { get; set; } = null!;

    public string Destiny { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid DriverId { get; set; }

    public RideStatus Status {get; set;}

    public Guid ServicetypeId { get; set; }

    public decimal DistanceKm { get; set; }

    public decimal TotalValue { get; set; }
    
    public PaymentMethod PaymentMethod { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual TbDriver Driver { get; set; } = null!;

    public virtual TbServicetype Servicetype { get; set; } = null!;

    public virtual ICollection<TbRating> TbRatings { get; set; } = new List<TbRating>();

    public virtual TbUser User { get; set; } = null!;
}
