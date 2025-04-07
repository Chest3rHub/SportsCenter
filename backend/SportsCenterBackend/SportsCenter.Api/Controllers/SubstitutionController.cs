using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Application.Exceptions.SubstitutionsExceptions;
using SportsCenter.Application.Substitutions.Commands.AssignSubstitution;
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
            catch (ActivityCanceledException ex)
            {
                return Conflict(new { message = ex.Message });
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
        public async Task<IActionResult> GetSubstitutionRequests([FromQuery] int offset = 0)
        {
            return Ok(await Mediator.Send(new GetSubstitutionRequests(offset)));
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
        [HttpGet("get-free-trainers-for-substitution")]
        public async Task<IActionResult> GetFreeTrainersForSubstitutions([FromQuery] DateTime date, [FromQuery] TimeSpan startHour, [FromQuery] TimeSpan endHour, [FromQuery] int offset = 0)
        {
            var query = new GetFreeTrainersForSubstitution(date,startHour,endHour,offset);

            var availableTrainers = await Mediator.Send(query);

            if (availableTrainers == null || !availableTrainers.Any())
            {
                return NotFound(new { Message = "No avaliable trainers for the given time." });
            }

            return Ok(availableTrainers);
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
        [HttpPut("assing-substitution")]
        public async Task<IActionResult> AssingSubstitution([FromBody] AssignSubstitution request)
        {
            try
            {         
                var result = await Mediator.Send(request);

                return Ok(result);
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (NotTrainerEmployeeException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (SubstitutionNotFoundException ex)
            {             
                return NotFound(new { message = ex.Message });
            }
            catch (SportActivityNotFoundException ex)
            { 
                return NotFound(new { message = ex.Message });
            }
            catch (ReservationNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }    
            catch (EmployeeAlreadyDismissedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (TrainerNotAvaliableException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }
    }
}
