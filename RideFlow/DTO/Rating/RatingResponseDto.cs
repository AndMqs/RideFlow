namespace RideFlow.DTOs;

public class RatingResponseDto
{
    public Guid Id { get; set; }
    public Guid RideId { get; set; }
    public string NomePassageiro { get; set; } = string.Empty;
    public string NomeMotorista { get; set; } = string.Empty;
    public int Rate { get; set; }
    public string? Comment { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}