using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        [Authorize(Roles = "Wlasciciel,Klient,Pomoc sprzatajaca,Trener,Pracownik administracyjny")]
        [HttpGet("Schedule-info")]
        public async Task<IActionResult> GetScheduleInfo([FromQuery] int weekOffset = 0)
        {
            var query = new GetScheduleInfo(weekOffset);
            var result = await Mediator.Send(query);

            return Ok(result);
        }
    }
}
