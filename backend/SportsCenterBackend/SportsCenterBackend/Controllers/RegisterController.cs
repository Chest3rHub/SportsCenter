using Microsoft.AspNetCore.Mvc;
using SportsCenterBackend.DTOs;
using SportsCenterBackend.Entities;
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
    
    [HttpPost]
    public async Task<IActionResult> RegisterClientAsync([FromBody] RegisterClientDTO clientDto)
    {
        if (clientDto == null)
        {
            return BadRequest("Osoba nie może być null");
        }

        try
        {
            await _registerDbService.RegisterClientAsync(clientDto);
            return Ok($"Zarejestrowano pomyslnie osobe: {clientDto.Imie} {clientDto.Nazwisko}");
        }
        catch (Exception e)
        {
            return Conflict("Ten adres email jest juz zajety");
        }
        
    }
    
}