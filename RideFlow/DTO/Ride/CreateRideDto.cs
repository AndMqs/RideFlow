public class CreateRideDto
{
    public Guid UserId { get; set; }
    public string Startpoint { get; set; } = null!;
    public string Destiny { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string PaymentMethod { get; set; } = null!;
    public decimal Km { get; set; }
}