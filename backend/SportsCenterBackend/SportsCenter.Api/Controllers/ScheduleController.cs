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

        [Authorize(Roles = "Wlasciciel,Klient,Gosc,Pomoc sprzatajaca,Trener,Pracownik administracyjny")]
        [HttpGet("Schedule-info")]
        public async Task<IActionResult> GetScheduleInfo([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var query = new GetScheduleInfo(startDate, endDate);
            var result = await Mediator.Send(query);

            return Ok(result);
        }
    }
}
