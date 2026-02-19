using System;
using System.Collections.Generic;

namespace RideFlow.Models;

public partial class TbUser
{
    public Guid Id { get; set; }

    public string Nameuser { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Phoneuser { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TbRide> TbRides { get; set; } = new List<TbRide>();
}
