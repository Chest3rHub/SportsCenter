using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SubstitutionsExceptions;
using SportsCenter.Application.Substitutions.Commands.ReportSubstitutionForActivities;
using SportsCenter.Application.Substitutions.Commands.ReportSubstitutionForReservation;
using SportsCenter.Application.Substitutions.Queries.GetFreeTrainersForSubstitution;
using SportsCenter.Application.Substitutions.Queries.GetSubstitutionRequestsForActivities;

namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class SubstitutionController : BaseController
    {
        public SubstitutionController(IMediator mediator) : base(mediator)
        {

        }

        [Authorize(Roles = "Trener")]
        [HttpPost("report-substitution-for-activity")]
        public async Task<IActionResult> ReportSubstitutionForActivities([FromBody] ReportSubstitutionForActivities request)
        {
            try
            {
                return Ok(await Mediator.Send(request));
            }
            catch (SportActivityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (SubstitutionForActivitiesNotAllowedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DuplicateSubstitutionActivityRequestException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });

            }
        }

        [Authorize(Roles = "Trener")]
        [HttpPost("report-substitution-for-reservation")]
        public async Task<IActionResult> ReportSubstitutionForReservation([FromBody] ReportSubstitutionForReservation request)
        {
            try
            {
                return Ok(await Mediator.Send(request));
            }
            catch (ReservationNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (SubstitutionForReservationNotAllowedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DuplicateSubstitutionReservationRequestException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });

            }
        }
        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
        [HttpGet("get-substitution-requests")]
        public async Task<IActionResult> GetSubstitutionRequests()
        {
            return Ok(await Mediator.Send(new GetSubstitutionRequests()));
        }

       // [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
        [HttpGet("get-free-trainers-for-substitution")]
        public async Task<IActionResult> GetFreeTrainersForSubstitutions([FromQuery] DateTime date, [FromQuery] TimeSpan startHour, [FromQuery] TimeSpan endHour)
        {
            var query = new GetFreeTrainersForSubstitution
            {
                Date = date,
                StartHour = startHour,
                EndHour = endHour
            };

            var availableTrainers = await Mediator.Send(query);

            if (availableTrainers == null || !availableTrainers.Any())
            {
                return NotFound(new { Message = "No avaliable trainers for the given time." });
            }

            return Ok(availableTrainers);
        }
    }
}
