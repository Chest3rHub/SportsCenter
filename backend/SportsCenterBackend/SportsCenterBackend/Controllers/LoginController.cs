using Microsoft.AspNetCore.Mvc;
using SportsCenterBackend.DTOs;
using SportsCenterBackend.Services;

namespace SportsCenterBackend.Controllers;

[ApiController]
[Route("[controller]")]

public class LoginController : ControllerBase
{
    private readonly ILoginDbService _logindbService;
    public LoginController(ILoginDbService loginDbService)
    {
        _logindbService= loginDbService;
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDTO login)
    {
        if (login == null)
        {
            return BadRequest("Login nie może być null");
        }

        try
        {
            var result = await _logindbService.LoginAsync(login);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
        
    }
    
}