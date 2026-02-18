namespace RideFlow.DTOs;

public class FinishedRideResponseDto
{
    public Guid Id { get; set; }
    public string NomePassageiro { get; set; } = null!;
    public string NomeMotorista { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string Status { get; set; } = string.Empty;
    public string FormaPagamento { get; set; } = null!;
    public string ValorAPagar { get; set; }
     public string Mensagem { get; set; } = string.Empty;
}