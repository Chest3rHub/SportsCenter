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
using SportsCenter.Application.Clients.Commands.AddDiscount;
using SportsCenter.Application.Clients.Commands.AddDepositYourself;
using SportsCenter.Application.Clients.Commands.UpdateClientDeposit;
using SportsCenter.Application.Clients.Commands.UpdateDiscount;
using static System.Net.Mime.MediaTypeNames;
using SportsCenter.Application.Clients.Queries.GetClientByEmail;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SportsCenter.Api.Controllers;

[Route("[controller]")]
public class ClientsController : BaseController
{
    public ClientsController(IMediator mediator) : base(mediator)
    {
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpGet("get-clients")]
    public async Task<IActionResult> GetClientsAsync([FromQuery] int offset = 0)
    {
        return Ok(await Mediator.Send(new GetClients(offset)));
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpGet("get-client-by-email")]
    public async Task<IActionResult> GetClientByEmailAsync([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email cannot be empty.");
        }
        var client = await Mediator.Send(new GetClientByEmail(email));

        if (client == null)
        {
            return NotFound($"No client found with email: {email}");
        }
        return Ok(client);
    }

    [AllowAnonymous]
    [HttpPost("register-client")]
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
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpPost("register-client-by-admin")]
    public async Task<IActionResult> RegisterClientByAdminAsync([FromBody] RegisterClient registerClient)
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
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpGet("byAge")]
    public async Task<IActionResult> GetClientsByAgeAsync([FromQuery] int minAge, [FromQuery] int maxAge, [FromQuery] int offset = 0)
    {
        var query = new GetClientsByAge(offset, minAge, maxAge);

        var clients = await Mediator.Send(query);
        return Ok(clients);
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpGet("byTags")]
    public async Task<IActionResult> GetClientsByTagsAsync([FromQuery] List<int> tagIds, [FromQuery] int offset = 0)
    {
        var query = new GetClientsByTags(offset, tagIds);

        var clients = await Mediator.Send(query);
        return Ok(clients);
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
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
        catch (TagNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpDelete("removeTags")]
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
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpPost("add-deposit-to-client")]
    public async Task<IActionResult> AddAccountDepositToClientAsync([FromBody] AddDepositToClient deposit)
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
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    //rozwiazanie bezfrontendowe tymczasowe wpisac w swaggerze w StripeToken "tok_visa"
    [Authorize(Roles = "Klient")]
    [HttpPost("add-deposit")]
    public async Task<IActionResult> AddAccountDepositYourselfAsync([FromBody] AddDepositYourself deposit)
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
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
    [HttpPut("update-client-deposit")]
    public async Task<IActionResult> UpdateClientDepositAsync([FromBody] UpdateClientDeposit deposit)
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
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
    [HttpPost("add-client-discount")]
    public async Task<IActionResult> AddDiscountAsync([FromBody] AddDiscount addDiscount)
    {
        try
        {            
            await Mediator.Send(addDiscount);
            return NoContent();
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
    [HttpPut("update-client-discount")]
    public async Task<IActionResult> UpdateDiscountAsync([FromBody] UpdateDiscount updateDiscount)
    {
        try
        {
            await Mediator.Send(updateDiscount);
            return NoContent();
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }
}

    

