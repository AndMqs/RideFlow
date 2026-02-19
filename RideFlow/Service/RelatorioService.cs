using System.Text;
using RideFlow.DTOs;
using RideFlow.Repositories; 
using RideFlow.Models;
namespace RideFlow.Service;

public class RelatorioService
{
    private readonly RideRepository _rideRepository;

    public RelatorioService(RideRepository rideRepository)
    {
        _rideRepository = rideRepository;
    }

    /// <summary>
    /// Gera um arquivo CSV com o histórico de corridas do usuário
    /// </summary>
    public async Task<byte[]> GerarCsvHistoricoUsuario(Guid userId, string nomeUsuario)
    {
        var historico = await _rideRepository.GetHistoricoByUserId(userId);
        
        if (historico == null || historico.Count == 0)
        {
            throw new Exception("Nenhuma corrida encontrada para este usuário.");
        }

        var sb = new StringBuilder();
        
        // Cabeçalho do CSV
        sb.AppendLine("ID Corrida;Data;Origem;Destino;Motorista;Placa;Categoria;Valor;Status;Avaliação;Comentário");
        
        // Linhas de dados
        foreach (var item in historico)
        {
            sb.AppendLine($"{item.Id};" +
                         $"{item.Data:dd/MM/yyyy HH:mm};" +
                         $"{item.Origem};" +
                         $"{item.Destino};" +
                         $"{item.NomeMotorista};" +
                         $"{item.Placa};" +
                         $"{item.Categoria};" +
                         $"{item.Valor:F2};" +
                         $"{item.Status};" +
                         $"{item.Avaliacao?.ToString() ?? "Não avaliado"};" +
                         $"{item.Comentario ?? "Sem comentário"}");
        }
        
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    /// <summary>
    /// Gera um arquivo CSV com resumo estatístico do usuário
    /// </summary>
    public async Task<byte[]> GerarCsvResumoUsuario(Guid userId, string nomeUsuario)
    {
        var historico = await _rideRepository.GetHistoricoByUserId(userId);
        
        if (historico == null || historico.Count == 0)
        {
            throw new Exception("Nenhuma corrida encontrada para este usuário.");
        }

        var sb = new StringBuilder();
        
        // Cabeçalho do resumo
        sb.AppendLine("RELATÓRIO DE VIAGENS");
        sb.AppendLine($"Usuário: {nomeUsuario}");
        sb.AppendLine($"Data de geração: {DateTime.Now:dd/MM/yyyy HH:mm}");
        sb.AppendLine("");
        
        // Estatísticas gerais
        sb.AppendLine("=== ESTATÍSTICAS GERAIS ===");
        sb.AppendLine($"Total de corridas: {historico.Count}");
        sb.AppendLine($"Total gasto: R$ {historico.Sum(x => x.Valor):F2}");
        sb.AppendLine($"Média por corrida: R$ {historico.Average(x => x.Valor):F2}");
        
        var corridasFinalizadas = historico.Count(x => x.Status == "finished");
        var corridasCanceladas = historico.Count(x => x.Status == "canceled");
        var corridasEmAndamento = historico.Count(x => x.Status == "in_progress");
        
        sb.AppendLine($"Corridas finalizadas: {corridasFinalizadas}");
        sb.AppendLine($"Corridas canceladas: {corridasCanceladas}");
        sb.AppendLine($"Corridas em andamento: {corridasEmAndamento}");
        
        // Média de avaliações
        var avaliacoes = historico.Where(x => x.Avaliacao.HasValue).Select(x => x.Avaliacao.Value).ToList();
        if (avaliacoes.Any())
        {
            sb.AppendLine($"Média de avaliações: {avaliacoes.Average():F2} estrelas");
        }
        
        sb.AppendLine("");
        sb.AppendLine("=== DETALHAMENTO DAS CORRIDAS ===");
        sb.AppendLine("Data;Origem;Destino;Motorista;Valor;Status;Avaliação");
        
        foreach (var item in historico.OrderByDescending(x => x.Data))
        {
            sb.AppendLine($"{item.Data:dd/MM/yyyy HH:mm};" +
                         $"{item.Origem};" +
                         $"{item.Destino};" +
                         $"{item.NomeMotorista};" +
                         $"R$ {item.Valor:F2};" +
                         $"{item.Status};" +
                         $"{item.Avaliacao?.ToString() ?? "-"}");
        }
        
        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}