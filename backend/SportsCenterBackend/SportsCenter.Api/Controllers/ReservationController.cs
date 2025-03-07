using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Reservations.Commands.MoveReservation;
using SportsCenter.Application.Reservations.Commands.RemoveReservation;
using SportsCenter.Application.Reservations.Commands.AddRecurringReservation;
using SportsCenter.Application.Reservations.Queries.GetReservationSummary;
using Microsoft.AspNetCore.Authorization;
using SportsCenter.Application.Reservations.Commands.AddSingleReservationYourself;
using SportsCenter.Application.Reservations.Commands.UpdateReservation;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.EmployeesException;

namespace SportsCenter.Api.Controllers;

[Route("[controller]")]
public class ReservationController : BaseController
{
    public ReservationController(IMediator mediator) : base(mediator)
    {
    }

    [Authorize(Roles = "Pracownik administracyjny, Wlasciciel")]
    [HttpPost("Create-single-reservation-for-client")]
    public async Task<IActionResult> CreateSingleReservationForClient([FromBody] AddSingleReservation addReservation)
    {

        var validationResults = new AddSingleReservationValidator().Validate(addReservation);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        try
        {
            await Mediator.Send(addReservation);
            return Ok(new { Message = "Reservation created successfully" });
        }
        catch (TooManyParticipantsException)
        {           
            return BadRequest(new { Message = "Too many participants. The maximum is 8." });
        }
        catch (CourtNotAvaliableException)
        {             
            return Conflict(new { Message = $"The court {addReservation.CourtId} is not available for the requested time." });
        }
        catch (TrainerNotAvaliableException)
        {               
            return Conflict(new { Message = "The selected trainer is not available for the requested time." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
        }
    }

    [Authorize(Roles = "Klient")]
    [HttpPost("Create-single-reservation-yourself")]
    public async Task<IActionResult> CreateSingleReservationYourself([FromBody] AddSingleReservationYourself addReservation)
    {

        var validationResults = new AddSingleReservationYourselfValidator().Validate(addReservation);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        try
        {
            await Mediator.Send(addReservation);
            return Ok(new { Message = "Reservation created successfully" });
        }
        catch (ReservationOutsideWorkingHoursException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (TooManyParticipantsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (CourtNotAvaliableException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (EmployeeNotFoundException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (NotTrainerEmployeeException ex)
        {
            return Conflict(new { message = ex.Message });
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
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
        }
    }

    [Authorize(Roles = "Pracownik administracyjny, Wlasciciel")]
    [HttpPost("Create-recurring-reservation")]
    public async Task<IActionResult> CreateRecurringReservation([FromBody] AddRecurringReservation addRecurringReservation)
    {      
        var validationResults = new AddRecurringReservationValidator().Validate(addRecurringReservation);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        try
        {           
            await Mediator.Send(addRecurringReservation);
            return Ok(new { Message = "Recurring reservation created successfully" });
        }
        catch (TooManyParticipantsException)
        {
            return BadRequest(new { Message = "Too many participants. The maximum is 8." });
        }
        catch (CourtNotAvaliableException)
        {
            return Conflict(new { Message = $"The court {addRecurringReservation.CourtId} is not available for the requested time." });
        }
        catch (TrainerNotAvaliableException)
        {
            return Conflict(new { Message = "The selected trainer is not available for the requested time." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
        }
    }

    [Authorize(Roles = "Klient")]
    [HttpPut("Move-reservation")]
    public async Task<IActionResult> MoveReservation([FromBody] MoveReservation moveReservation)
    {
        var validationResults = new MoveReservationValidator().Validate(moveReservation);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        try
        {
            await Mediator.Send(moveReservation);
            return Ok(new { Message = "Reservation moved successfully." });
        }
        catch (ReservationNotFoundException)
        {
            return NotFound(new { Message = "Reservation not found." });
        }
        catch (CourtNotAvaliableException)
        {
            return Conflict(new { Message = "The court is not available for the new time." });
        }
        catch (TrainerNotAvaliableException)
        {
            return Conflict(new { Message = "The trainer is not available for the new time." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [Authorize(Roles = "Klient, Wlasciciel")]
    [HttpDelete("{reservationId}")]
    public async Task<IActionResult> RemoveReservation(int reservationId, CancellationToken cancellationToken)
    {
        try
        {
            var request = new RemoveReservation(reservationId);
            await Mediator.Send(request, cancellationToken);
            return Ok("Reservation canceled successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ReservationNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
    [HttpPut("Update-trainer-in-reservation")]
    public async Task<IActionResult> UpdateTrainerInReservation([FromBody] UpdateReservation updateReservation)
    {
        try
        {
            await Mediator.Send(updateReservation);
            return Ok(new { Message = "Reservation updated successfully." });
        }
        catch (ReservationNotFoundException)
        {
            return NotFound(new { Message = "Reservation not found." });
        }
        catch (TrainerNotAvaliableException)
        {
            return NotFound(new { Message = "Trainer not avaliable." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [Authorize(Roles = "Wlasciciel")]
    [HttpGet("Reservation-summary")]
    public async Task<IActionResult> GetReservationSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (startDate > endDate) //nie jestem pewna gdzie ten warunek umiescic
        {
            return BadRequest("StartDate cannot be greater than EndDate.");
        }

        var query = new GetReservationSummary(startDate, endDate);
        var result = await Mediator.Send(query);

        return Ok(result);
    }
}
