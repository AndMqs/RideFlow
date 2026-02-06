using System;
using System.Collections.Generic;

namespace RideFlow.Models;

public partial class TbServicetype
{
    public Guid Id { get; set; }

    public virtual ICollection<TbRide> TbRides { get; set; } = new List<TbRide>();

    public virtual ICollection<TbDriver> Drivers { get; set; } = new List<TbDriver>();
}
