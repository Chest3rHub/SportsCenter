using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Clients.Queries.GetClients;
using SportsCenter.Application.Exceptions.SportsCenterExceptions;
using SportsCenter.Application.SportsCenterManagement.Commands.SetSpecialSportsCenterWorkingHours;
using SportsCenter.Application.SportsCenterManagement.Queries.GetAvailableCourts;
using SportsCenter.Application.SportsCenterManagement.Queries.GetCourts;
using SportsCenter.Application.SportsCenterManagement.Queries.GetSportsCenterWorkingHours;
using SportsCenter.Application.SportsClubManagement.Commands.AddSportsClubWorkingHours;

namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class SportsCenterManagementController : BaseController
    {
        public SportsCenterManagementController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpPost("Set-sports-center-working-hours")]
        public async Task<IActionResult> SetSportsCenterWorkingHoursAsync([FromBody] SetSportsCenterWorkingHours workingHours)
        {
            var validationResults = new SetSportsCenterWorkingHoursValidator().Validate(workingHours);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

            try
            {
                await Mediator.Send(workingHours);
                return NoContent();
            }
            catch (DayOfWeekNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpPost("Set-special-sports-center-working-hours")]
        public async Task<IActionResult> SetSpecialSportsCenterWorkingHoursAsync([FromBody] SetSpecialSportsCenterWorkingHours workingHours)
        {
            var validationResults = new SetSpecialSportsCenterWorkingHoursValidator().Validate(workingHours);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

            try
            {
                await Mediator.Send(workingHours);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [HttpGet("get-sports-club-working-hours")]
        public async Task<IActionResult> GetWorkingHoursForWeek([FromQuery] int weekOffset)
        {
            var workingHours = await Mediator.Send(new GetSportsCenterWorkingHours(weekOffset));
            return Ok(workingHours);
        }


        [HttpGet("get-sports-club-courts")]
        public async Task<IActionResult> GetCourts()
        {
            var courts = await Mediator.Send(new GetCourts());
            return Ok(courts);
        }

        [HttpGet("get-available-sports-club-courts")]
        public async Task<IActionResult> GetAvailableCourts([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            var courts = await Mediator.Send(new GetAvailableCourts(startTime, endTime));
            return Ok(courts);
        }
    }
}
