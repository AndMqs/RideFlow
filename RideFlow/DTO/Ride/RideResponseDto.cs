public class RideResponseDto
{
    public Guid Id { get; set; }
    public string NomePassageiro { get; set; } = null!;
    public string NomeMotorista { get; set; } = null!;
    public string Placa { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public decimal ValorCorrida { get; set; }
    public string Origem { get; set; } = null!;
    public string Destino { get; set; } = null!;
    public string FormaPagamento { get; set; } = null!;
    public string Status { get; set; } = string.Empty;
}