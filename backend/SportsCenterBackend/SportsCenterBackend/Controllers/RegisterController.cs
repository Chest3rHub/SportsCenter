using Microsoft.AspNetCore.Mvc;
using SportsCenterBackend.Services;

namespace SportsCenterBackend.Controllers;

[ApiController]
[Route("[controller]")]

public class RegisterController : ControllerBase
{
    private readonly IRegisterDbService _registerDbService;
    public RegisterController(IRegisterDbService registerDbService)
    {
        _registerDbService= registerDbService;
    }
    
    
}