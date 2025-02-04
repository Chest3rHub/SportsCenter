using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Schedule.Queries.GetScheduleInfo;


namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class ScheduleController : BaseController
    {
        public ScheduleController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet("Schedule-info")]
        public async Task<IActionResult> GetScheduleInfo([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var query = new GetScheduleInfo(startDate, endDate);
            var result = await Mediator.Send(query);

            return Ok(result);
        }
    }
}
