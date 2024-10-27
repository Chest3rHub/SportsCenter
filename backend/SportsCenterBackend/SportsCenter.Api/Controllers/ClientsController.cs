using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Users.Commands;
using SportsCenter.Application.Users.Commands.Login;
using SportsCenter.Application.Users.Commands.RegisterClient;
using SportsCenter.Application.Users.Queries;
using SportsCenter.Application.Users.Queries.GetClients;
using SportsCenter.Infrastructure.DAL;
using SportsCenter.Application.Exceptions;

namespace SportsCenter.Api.Controllers;

[Route("[controller]")]
public class ClientsController : BaseController
{
    public ClientsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> RegisterClientAsync()
    {
        return Ok(await Mediator.Send(new GetClients()));
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterClientAsync([FromBody] RegisterClient registerClient)
    {
        var validationResults = new RegisterClientValidator().Validate(registerClient);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        try
        {
            await Mediator.Send(registerClient);
            return NoContent();
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Wystąpił błąd podczas wysyłania żądania", details = ex.Message });
        }
    }


    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] Login loginCommand)
    {
        var loginResponse = await Mediator.Send(loginCommand);
        return Ok(loginResponse);
    }
}