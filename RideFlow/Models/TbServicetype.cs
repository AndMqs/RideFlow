using System.ComponentModel.DataAnnotations.Schema;

namespace RideFlow.Models;

[Table("tb_servicetype")]
public partial class TbServicetype
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("category")]
    public ServiceCategory Category { get; set; }

    public virtual ICollection<TbDriver> Drivers { get; set; } = new List<TbDriver>();
    public virtual ICollection<TbRide> TbRides { get; set; } = new List<TbRide>();
}