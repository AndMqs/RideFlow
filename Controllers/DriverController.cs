using Microsoft.AspNetCore.Mvc;

namespace RideFlow.Controllers;

[ApiController]
[Route("driver")]
public class DriverController : ControllerBase
{
    
    private readonly DriverService _service;

    public DriverController(DriverService service)
    {
        _service = service;
    }

    [HttpPost(Name = "CreateDriver")]
    public IActionResult CreateDriver([FromBody] CreateDriverDto dto)    {
        try
        {
            _service.CreateDriver(dto);
            return Ok("Motorista cadastrado com sucesso.");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


    [HttpGet(Name = "GetAllDrivers")]
    public IActionResult GeAlltDrivers()
    {
        var driver = _service.GetDrivers();
        return Ok(driver);
    }

    

 
}
