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
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.CourtsExceptions;
using SportsCenter.Application.Reservations.Queries.GetYourReservations;
using SportsCenter.Application.Reservations.Commands.PayForReservation;
using SportsCenter.Application.Reservations.Commands.PayForClientReservation;
using SportsCenter.Application.Reservations.Commands.CancelReservation;
using SportsCenter.Application.Reservations.Commands.CancelClientReservation;
using SportsCenter.Application.Reservations.Queries.getCourtEvents;
using SportsCenter.Application.Reservations.Queries.GetReservation;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;

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
        catch (TooManyParticipantsException ex)
        {
            return StatusCode(415, new { message = ex.Message });
        }
        catch (CourtNotAvaliableException ex)
        {
            return StatusCode(411, new { message = ex.Message });
        }
        catch (TrainerNotAvaliableException ex)
        {
            return StatusCode(419, new { message = ex.Message });
        }
        catch (ClientAlreadyHasActivityOrReservationException ex)
        {
            return StatusCode(420, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
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
        catch (ClientAlreadyHasActivityOrReservationException ex)
        {
            return StatusCode(420, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
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
            if (HttpContext.Items.TryGetValue("reservationResult", out var result) && result is not null)
            {
                return Ok(result);
            }
            return StatusCode(500, new { message = "An error occurred while sending the request" });
        }
        catch (ClientWithGivenIdNotFoundException ex)
        {
            return StatusCode(414, new { message = ex.Message });
        }
        catch (TooManyParticipantsException ex)
        {
            return StatusCode(415, new { message = ex.Message });
        }
        catch (EmployeeNotFoundException ex)
        {
            return StatusCode(416, new { message = ex.Message });
        }
        catch (NotTrainerEmployeeException ex)
        {
            return StatusCode(417, new { message = ex.Message });
        }
        catch (EmployeeAlreadyDismissedException ex)
        {
            return StatusCode(418, new { message = ex.Message });
        }
        catch (ClientAlreadyHasActivityOrReservationException ex)
        {
            return StatusCode(420, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
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
        catch (ReservationNotFoundException ex)
        {
            return StatusCode(411, new { message = ex.Message });
        }
        catch (ReservationOutsideWorkingHoursException ex)
        {
            return StatusCode(412, new { message = ex.Message }); ;
        }
        catch (NotThatClientReservationException ex)
        {
            return StatusCode(413, new { message = ex.Message });
        }
        catch (PostponeReservationNotAllowedException ex)
        {
            return StatusCode(414, new { message = ex.Message });
        }
        catch (CourtNotAvaliableException ex)
        {
            return StatusCode(415, new { message = ex.Message });
        }
        catch (EmployeeNotFoundException ex)
        {
            return StatusCode(416, new { message = ex.Message });
        }
        catch (NotTrainerEmployeeException ex)
        {
            return StatusCode(417, new { message = ex.Message });
        }
        catch (EmployeeAlreadyDismissedException ex)
        {
            return StatusCode(418, new { message = ex.Message });
        }
        catch (TrainerNotAvaliableException ex)
        {
            return StatusCode(419, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (ClientAlreadyHasActivityOrReservationException ex)
        {
            return StatusCode(420, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
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
        catch (NotThatClientReservationException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
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
        catch (ReservationNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (EmployeeNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Wlasciciel")]
    [HttpGet("Reservation-summary")]
    public async Task<IActionResult> GetReservationSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int offset = 0)
    {
        if (startDate > endDate) //nie jestem pewna gdzie ten warunek umiescic
        {
            return BadRequest("StartDate cannot be greater than EndDate.");
        }

        var query = new GetReservationSummary(offset, startDate, endDate);
        var result = await Mediator.Send(query);

        return Ok(result);
    }

    [Authorize(Roles = "Klient")]
    [HttpGet("get-your-reservations")]
    public async Task<IActionResult> GetYourReservations([FromQuery] int offset = 0)
    {
        return Ok(await Mediator.Send(new GetYourReservations(offset)));
    }

    [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
    [HttpGet("get-all-reservations")]
    public async Task<IActionResult> GetAllReservations([FromQuery] int offset = 0)
    {
        return Ok(await Mediator.Send(new GetAllReservations(offset)));
    }

    [Authorize(Roles = "Klient")]
    [HttpPut("pay-for-reservation-with-balance-account")]
    public async Task<IActionResult> PayForReservation([FromBody] PayForReservation request)
    {
        try
        {
            await Mediator.Send(request);
            return Ok(new { message = "Successfully paid for reservation." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ReservationNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ClientWithGivenIdNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ReservationAlreadyPaidException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (PaymentFailedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }
    [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
    [HttpPut("pay-for-client-reservation")]
    public async Task<IActionResult> PayForClientReservation([FromBody] PayForClientReservation request)
    {
        try
        {
            await Mediator.Send(request);
            return Ok(new { message = "Successfully paid for client reservation." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (ReservationNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ReservationAlreadyPaidException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ReservationAlreadyCanceledException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (PaymentFailedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }
    [Authorize(Roles = "Klient")]
    [HttpPut("cancel-reservation")]
    public async Task<IActionResult> CancelReservationAsync([FromBody] CancelReservation cancelReservaion)
    {     
        try
        {
            await Mediator.Send(cancelReservaion);
            return Ok(new { Message = "Successfully canceled reservation." });
        }
        catch (ReservationNotFoundException ex)
        {
            return StatusCode(411, new { message = ex.Message });
        }
        catch (TooLateToCancelreservationException ex)
        {
            return StatusCode(412, new { message = ex.Message });
        }
        catch (ReservationAlreadyCanceledException ex)
        {
            return StatusCode(413, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }
    [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
    [HttpPut("cancel-client-reservation")]
    public async Task<IActionResult> CancelClientReservationAsync([FromBody] CancelClientReservation cancelReservaion)
    {
        try
        {
            await Mediator.Send(cancelReservaion);
            return Ok(new { Message = "Successfully canceled client reservation." });
        }
        catch (ClientNotFoundException ex)
        {
            return StatusCode(411, new { message = ex.Message });
        }
        catch (ReservationNotFoundException ex)
        {
            return StatusCode(412, new { message = ex.Message });
        }
        catch (ReservationAlreadyCanceledException ex)
        {
            return StatusCode(413, new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }
    [HttpGet("get-court-events")]
    public async Task<IActionResult> getCourtEvents([FromQuery] int courtId, [FromQuery] DateTime date)
    {
        return Ok(await Mediator.Send(new getCourtEvents(courtId, date)));
    }

    [Authorize(Roles = "Pracownik administracyjny, Wlasciciel")]
    [HttpGet("get-reservation-with-id-{reservationId}")]
    public async Task<IActionResult> GetReservationByIdAsync(int reservationId)
    {
        var reservation = await Mediator.Send(new GetReservation(reservationId));
        return Ok(reservation);
    }
}
