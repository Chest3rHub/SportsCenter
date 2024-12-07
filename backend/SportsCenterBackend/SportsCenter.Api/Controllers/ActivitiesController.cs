using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Activities.Commands;
using SportsCenter.Application.Activities.Commands.AddSportActivity;
using SportsCenter.Application.Activities.Commands.RemoveSportActivity;
using SportsCenter.Application.Activities.Queries;

namespace SportsCenter.Api.Controllers;

    [Route("[controller]")]
    public class ActivitiesController : BaseController
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Add-activity")]
        public async Task<IActionResult> AddActivity([FromBody] AddSportActivity command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        
        [HttpGet("Get-sport-activities")]
        public async Task<IActionResult> GetAllSportActivitiesAsync()
        {
            var activities = await Mediator.Send(new GetSportActivities());
            return Ok(activities);
        }
        

        [HttpGet("{sportActivityId}")]
        public async Task<IActionResult> GetSportActivityByIdAsync(int sportActivityId)
        {
            var activity = await Mediator.Send(new GetSportActivityById(sportActivityId));
            return Ok(activity);
        }
        
        [HttpDelete("{sportActivityId}")]
        public async Task<IActionResult> RemoveSportActivityAsync(int sportActivityId)
        {
            await Mediator.Send(new RemoveSportActivity(sportActivityId));
            return NoContent();
        }
    }

