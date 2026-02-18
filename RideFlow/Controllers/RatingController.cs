using Microsoft.AspNetCore.Mvc;
using RideFlow.DTOs; 
using RideFlow.Service;

namespace RideFlow.Controllers;

[ApiController]
[Route("rating")]
public class RatingController : ControllerBase
{
    private readonly RatingService _ratingService;

    public RatingController(RatingService ratingService)
    {
        _ratingService = ratingService;
    }

    /// <summary>
    /// Criar uma nova avaliação para uma corrida finalizada
    /// </summary>
    [HttpPost(Name="CreateRating")]
    public async Task<IActionResult> CreateRating([FromBody] CreateRatingDto dto)
    {
        try
        {
            var result = await _ratingService.CreateRating(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    /// <summary>
    /// Listar todas as avaliações de um motorista específico
    /// </summary>
    [HttpGet("driver/{driverId}", Name = "GetRatingsByDriver")]
    public async Task<IActionResult> GetRatingsByDriver(Guid driverId)
    {
        try
        {
            var result = await _ratingService.GetRatingsByDriver(driverId);
            
            // Retorna lista vazia se não houver avaliações (sem mensagem extra)
            return Ok(result ?? new List<RatingResponseDto>());
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    /// <summary>
    /// Obter a média de avaliações de um motorista
    /// </summary>
    [HttpGet("driver/{driverId}/average", Name = "GetAverageRatingByDriver")]
    public async Task<IActionResult> GetAverageRatingByDriver(Guid driverId)
    {
        try
        {
            var average = await _ratingService.GetAverageRatingByDriver(driverId);
            
            // Busca as avaliações para saber o total (opcional, só para enriquecer a resposta)
            var ratings = await _ratingService.GetRatingsByDriver(driverId);
            
            return Ok(new 
            { 
                driverId = driverId,
                media = Math.Round(average, 2),
                totalAvaliacoes = ratings?.Count ?? 0
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}