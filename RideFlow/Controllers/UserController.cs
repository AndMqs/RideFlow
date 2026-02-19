using Microsoft.AspNetCore.Mvc;

namespace RideFlow.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    
    private readonly UserService _service;

    public UserController(UserService service)
    {
        _service = service;
    }

    [HttpPost(Name = "CreateUser")]
    public IActionResult CreateUser([FromBody] CreateUserDto dto)    {
      
      _service.CreateUser(dto);
      return Ok("Usuário cadastrado com sucesso.");
    }

    [HttpGet(Name = "GetAllUsers")]
    public IActionResult GeAlltUsers()
    {
        var user = _service.GetUsers();
        return Ok(user);
    }

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
}
