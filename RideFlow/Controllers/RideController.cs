using Microsoft.AspNetCore.Mvc;
using RideFlow.DTOs;
using RideFlow.Repositories;
using RideFlow.Service;

namespace RideFlow.Controllers;

[ApiController]
[Route("ride")]
public class RideController : ControllerBase
{
    private readonly RideService _service;
    private readonly UserRepository _userRepository;
    private readonly RelatorioService _relatorioService;


    public RideController(RideService service, UserRepository userRepository, RelatorioService relatorioService)
    {
        _service = service;
        _userRepository = userRepository;
        _relatorioService = relatorioService;
    }

    [HttpPost(Name = "CreateRide")]
    public IActionResult CreateRide([FromBody] CreateRideDto dto)
    {
        try
        {
            var rideResponse = _service.CreateRide(dto);
            return Ok(rideResponse);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("status/{status}")]
    public IActionResult GetRidesByStatus(string status)
    {
        try
        {
            var rides = _service.GetRidesByStatus(status);
            return Ok(rides);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("all", Name = "GetAllRides")]
    public async Task<IActionResult> GetAllRides()
    {
        try
        {
            var rides = await _service.GetAllRiders();
            return Ok(rides);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("cancel", Name = "CancelRide")]
    public async Task<IActionResult> CancelRide([FromBody] CancelRideDto dto)
    {
        try
        {
            var cancelResponse = await _service.CancelRide(dto.RideId);
            return Ok(cancelResponse);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("finish", Name = "FinishRide")]
    public async Task<IActionResult> FinishRide([FromBody] FinishedRideDto dto)
    {
        try
        {
            var finishResponse = await _service.FinishRide(dto.RideId);
            return Ok(finishResponse);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
/// Gera relatório CSV com histórico detalhado de corridas do usuário
/// </summary>
[HttpGet("relatorio/{userId}/detalhado")]
public async Task<IActionResult> GerarRelatorioDetalhado(Guid userId, [FromQuery] string? nomeUsuario)
{
    try
    {
        // Se não fornecer nome, busca no banco
        if (string.IsNullOrEmpty(nomeUsuario))
        {
            var user = await _userRepository.GetByIdAsync(userId);
            nomeUsuario = user?.Nameuser ?? "Usuário";
        }
        
        var csvBytes = await _relatorioService.GerarCsvHistoricoUsuario(userId, nomeUsuario);
        
        // Nome do arquivo
        var fileName = $"historico_{nomeUsuario.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        
        // Retornar como arquivo para download
        return File(csvBytes, "text/csv", fileName);
    }
    catch (Exception ex)
    {
        return BadRequest(new { erro = ex.Message });
    }
}

/// <summary>
/// Gera relatório CSV com resumo estatístico do usuário
/// </summary>
[HttpGet("relatorio/{userId}/resumo")]
public async Task<IActionResult> GerarRelatorioResumo(Guid userId, [FromQuery] string? nomeUsuario)
{
    try
    {
        // Se não fornecer nome, busca no banco
        if (string.IsNullOrEmpty(nomeUsuario))
        {
            var user = await _userRepository.GetByIdAsync(userId);
            nomeUsuario = user?.Nameuser ?? "Usuário";
        }
        
        var csvBytes = await _relatorioService.GerarCsvResumoUsuario(userId, nomeUsuario);
        
        // Nome do arquivo
        var fileName = $"resumo_{nomeUsuario.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        
        // Retornar como arquivo para download
        return File(csvBytes, "text/csv", fileName);
    }
    catch (Exception ex)
    {
        return BadRequest(new { erro = ex.Message });
    }
}
}
