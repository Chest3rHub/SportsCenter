using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Clients.Commands.AddClientTags;
using SportsCenter.Application.Clients.Commands.AddDeposit;
using SportsCenter.Application.Clients.Commands.RegisterClient;
using SportsCenter.Application.Clients.Commands.RemoveClientTags;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Clients.Queries.GetClients;
using SportsCenter.Application.Clients.Queries.GetClientsByAge;
using SportsCenter.Application.Clients.Queries.GetClientsByTags;

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
    public async Task<IActionResult> GetClientAsync([FromBody] RegisterClient registerClient)
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

    [HttpGet("byAge")]
    public async Task<IActionResult> GetClientsByAgeAsync([FromQuery] int minAge, [FromQuery] int maxAge)
    {
        var query = new GetClientsByAge
        {
            MinAge = minAge,
            MaxAge = maxAge
        };

        var clients = await Mediator.Send(query);
        return Ok(clients);
    }

    [HttpGet("byTags")]
    public async Task<IActionResult> GetClientsByTagsAsync([FromQuery] List<int> tagIds)
    {
        var query = new GetClientsByTags
        {
            TagIds = tagIds
        };

        var clients = await Mediator.Send(query);
        return Ok(clients);
    }

    [HttpPost("accountDeposit")]
    public async Task<IActionResult> AddAccountDepositAsync([FromBody] AddDeposit deposit)
    {
        try
        {
            await Mediator.Send(deposit);
            return NoContent();
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Wystąpił błąd podczas doładowania salda", details = ex.Message });
        }
    }
    
    [HttpPost("addTags")]
    public async Task<IActionResult> AddClientTagsAsync([FromBody] AddClientTags addClientTags)
    {
        
        try
        {
            await Mediator.Send(addClientTags);
            return NoContent();
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (TagLimitException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Wystąpił błąd podczas dodawania tagów", details = ex.Message });
        }
    }

    [HttpPost("removeTags")]
    public async Task<IActionResult> RemoveClientTagsAsync([FromBody] RemoveClientTags removeClientTags)
    {
        var validationResults = new RemoveClientTagsValidator().Validate(removeClientTags);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }
        
        try
        {
            await Mediator.Send(removeClientTags);
            return NoContent();
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Wystąpił błąd podczas usuwania tagów", details = ex.Message });
        }
    }

    

}