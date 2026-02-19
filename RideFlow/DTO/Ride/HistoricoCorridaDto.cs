namespace RideFlow.DTOs;

public class HistoricoCorridaDto
{
    /// <summary>
    /// ID da corrida
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome do passageiro
    /// </summary>
    public string NomePassageiro { get; set; } = string.Empty;

    /// <summary>
    /// Nome do motorista
    /// </summary>
    public string NomeMotorista { get; set; } = string.Empty;

    /// <summary>
    /// Placa do veículo
    /// </summary>
    public string Placa { get; set; } = string.Empty;

    /// <summary>
    /// Categoria da corrida (basic, premium, vip)
    /// </summary>
    public string Categoria { get; set; } = string.Empty;

    /// <summary>
    /// Valor total da corrida
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Ponto de partida
    /// </summary>
    public string Origem { get; set; } = string.Empty;

    /// <summary>
    /// Ponto de destino
    /// </summary>
    public string Destino { get; set; } = string.Empty;

    /// <summary>
    /// Status da corrida (in_progress, finished, canceled)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Data e hora da corrida
    /// </summary>
    public DateTime Data { get; set; }

    /// <summary>
    /// Nota dada pelo passageiro (1 a 5) - pode ser nulo se não avaliado
    /// </summary>
    public int? Avaliacao { get; set; }

    /// <summary>
    /// Comentário da avaliação - opcional
    /// </summary>
    public string? Comentario { get; set; }
}