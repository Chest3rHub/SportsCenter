using Microsoft.AspNetCore.Mvc;
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
    
    
}