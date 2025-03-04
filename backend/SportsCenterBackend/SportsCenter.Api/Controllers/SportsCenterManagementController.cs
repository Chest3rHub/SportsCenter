using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.SportsCenterExceptions;
using SportsCenter.Application.SportsCenterManagement.Commands.SetSpecialSportsCenterWorkingHours;
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
    }
}
