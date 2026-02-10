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

    /*

    [HttpPatch("{id}", Name = "UpdateUsers")]
    public IActionResult UpdateUser( Guid Id, [FromBody] UpdateUserDto dto)
    {
        var user = _service.UpdateUser(Id, dto);

        if (user == null)
            return NotFound("Usuário não encontrado");

        return Ok(user);
    }

  [HttpDelete("{id}", Name = "DeleteUsers")]
   public IActionResult DeleteUser(Guid Id)
   {
        try
        {
            _service.DeleteUser(Id);
            return NoContent();
        }
        catch(Exception ex)
        {
            return NotFound(ex);
        }
        
   }

   */
}
