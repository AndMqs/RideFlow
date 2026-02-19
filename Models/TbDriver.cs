using System;
using System.Collections.Generic;

namespace RideFlow.Models;

public partial class TbDriver
{
    public Guid Id { get; set; }

    public string Namedriver { get; set; } = null!;

    public string Cnh { get; set; } = null!;

    public string Plate { get; set; } = null!;

    public int Yearcar { get; set; }

    public string Modelcar { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TbRide> TbRides { get; set; } = new List<TbRide>();

    public virtual ICollection<TbServicetype> Servicetypes { get; set; } = new List<TbServicetype>();
}
