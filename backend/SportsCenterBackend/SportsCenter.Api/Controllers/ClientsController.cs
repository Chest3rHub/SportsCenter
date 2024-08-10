using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Users.Commands;
using SportsCenter.Application.Users.Commands.RegisterClient;
using SportsCenter.Application.Users.Queries;
using SportsCenter.Application.Users.Queries.GetClients;
using SportsCenter.Infrastructure.DAL;

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
        await Mediator.Send(registerClient);
        return NoContent();
    }
}