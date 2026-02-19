using System;
using System.Collections.Generic;

namespace RideFlow.Models;

public partial class TbRating
{
    public Guid Id { get; set; }

    public int Rate { get; set; }

    public string Comment { get; set; } = null!;

    public Guid RideId { get; set; }

    public virtual TbRide Ride { get; set; } = null!;
}
