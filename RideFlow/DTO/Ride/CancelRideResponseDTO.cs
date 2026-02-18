namespace RideFlow.DTOs;

public class CancelRideResponseDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ValorOriginal { get; set; }
    public string ValorCancelamento { get; set; } 
    public string ValorAPagar { get; set; }
     public string Mensagem { get; set; } = string.Empty;
}