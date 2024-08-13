using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SportsCenter.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IMediator Mediator { get; }

    public BaseController(IMediator mediator)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }
}