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


    public RideController(RideService service, DriverRepository driverRepository)
    {
        _service = service;
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
    
}
