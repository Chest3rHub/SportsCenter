using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Application.Reservations.Commands.MoveReservation;
using SportsCenter.Application.Reservations.Commands.RemoveReservation;
using SportsCenter.Application.Reservations.Commands.AddRecurringReservation;

namespace SportsCenter.Api.Controllers;

[Route("[controller]")]
public class ReservationController : BaseController
{
    public ReservationController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("Create-single-reservation")]
    public async Task<IActionResult> CreateSingleReservation([FromBody] AddSingleReservation addReservation)
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
}
