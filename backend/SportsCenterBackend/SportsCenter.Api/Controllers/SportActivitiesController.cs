using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Activities.Commands.AddSportActivity;
using SportsCenter.Application.Activities.Commands.CancelSportActivity;
using SportsCenter.Application.Activities.Commands.PayForActivity;
using SportsCenter.Application.Activities.Commands.PayForClientActivity;
using SportsCenter.Application.Activities.Commands.RemoveSportActivity;
using SportsCenter.Application.Activities.Commands.SignUpForActivity;
using SportsCenter.Application.Activities.Queries;
using SportsCenter.Application.Activities.Queries.GetActivitySummary;
using SportsCenter.Application.Activities.Queries.GetAllSportActivities;
using SportsCenter.Application.Activities.Queries.GetSportActivity;
using SportsCenter.Application.Activities.Queries.GetYourSportActivities;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.CourtsExceptions;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using SportsCenter.Application.Activities.Queries.GetYourUpcomingActivities;

namespace SportsCenter.Api.Controllers;

    [Route("[controller]")]
    public class SportActivitiesController : BaseController
    {

    public SportActivitiesController(IMediator mediator) : base(mediator)
    {
    }

    [Authorize(Roles = "Wlasciciel")]
    [HttpPost("Add-activity")]
        public async Task<IActionResult> AddActivity([FromBody] AddSportActivity addSportActivity)
        {
        var validationResults = new AddSportActivityValidator().Validate(addSportActivity);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }
        try
        {
            await Mediator.Send(addSportActivity);
            return Ok(new { Message = "Sport activity added successfully." });
        }
        catch (ActivityOutsideWorkingHoursException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (WrongCourtNameException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (InvalidDayOfWeekException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (CourtNotAvaliableException ex)
        {
            return Conflict(new { message = ex.Message });
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
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [HttpGet("Get-schedule-activities")]
    public async Task<IActionResult> GetAllSportActivitiesAsync([FromQuery] int weekOffset = 0)
        {
            var activities = await Mediator.Send(new GetAllSportActivities(weekOffset));
            return Ok(activities);
        }

    [HttpGet("get-sport-activity-with-id-{sportActivityId}")]
        public async Task<IActionResult> GetSportActivityByIdAsync(int sportActivityId)
        {
            var activity = await Mediator.Send(new GetSportActivity(sportActivityId));
            return Ok(activity);
        }

    [Authorize(Roles = "Wlasciciel")]
    [HttpDelete("remove-sport-activity")]
    public async Task<IActionResult> RemoveSportActivityAsync([FromBody] RemoveSportActivity removeSportActivity)
    {
        var validationResults = new RemoveSportActivityValidator().Validate(removeSportActivity);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }
        try
        {
            await Mediator.Send(removeSportActivity);
            return Ok(new { Message = "Sport activity removed successfully." });
        }
        catch (SportActivityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    [Authorize(Roles = "Klient")]
    [HttpPost("sign-up-for-activity")]
    public async Task<IActionResult> SignUpForActivityAsync([FromBody] SignUpForActivity signUpForActivity)
    {
        var validationResults = new SignUpForActivityValidator().Validate(signUpForActivity);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }
        try
        {
            await Mediator.Send(signUpForActivity);
            return Ok(new { Message = "Successfully signed up for the activity." });
        }
        catch (SportActivityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidDayOfWeekException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ActivityCanceledException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ActivityTimeTooFarException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ClientAlreadySignedUpException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }

    // dziala dopiro jak zrobimy get schedule activities bo 
    //po wywolaniu tego uzupelniaja sie rekordy w tabeli InstancjaZajec i mozna
    //dla danej daty wybrac zajecia(na frontendzie to bedzie mialo wiekszy sens
    //niz sie teraz wydaje ze ma)
    [Authorize(Roles = "Wlasciciel")]
    [HttpPut("cancel-activity-instance")]
    public async Task<IActionResult> CancelActivityAsync([FromBody] CancelSportActivity cancelSportActivity)
    {
        var validationResults = new CancelSportActivityValidator().Validate(cancelSportActivity);
        if (!validationResults.IsValid)
        {
            return BadRequest(validationResults.Errors);
        }

        try
        {
            await Mediator.Send(cancelSportActivity);
            return Ok(new { Message = "Successfully canceled the activity instance." });
        }
        catch (SportActivityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidActivityDateException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ActivityAlreadyCanceledException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (RefundAlreadyGivenException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
        }
    }
    [Authorize(Roles = "Wlasciciel")]
    [HttpGet("get-activity-summary")]
        public async Task<IActionResult> GetActivitySummaryAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate) //nie jestem pewna gdzie ten warunek umiescic
            {
                return BadRequest("StartDate cannot be greater than EndDate.");
            }

            var query = new GetActivitySummary(startDate, endDate);
            var result = await Mediator.Send(query);

            return Ok(result);
        }

    [Authorize(Roles = "Klient")]
    [HttpGet("get-your-activities")]
    public async Task<IActionResult> GetYourSportActivities([FromQuery] int offset = 0)
    {
        return Ok(await Mediator.Send(new GetYourSportActivities(offset)));
    }

    [Authorize(Roles = "Klient")]
    [HttpPut("pay-for-activity-with-balance-account")]
    public async Task<IActionResult> PayForActivity([FromBody] PayForActivity request)
    {
        try
        {        
            await Mediator.Send(request);
            return Ok(new { message = "Successfully paid for activity." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InstanceOfActivityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ClientWithGivenIdNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ClientWithdrawnException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ActivityAlreadyPaidException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ActivityCanceledException ex)
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
    [HttpGet("get-your-upcoming-activities")]
    public async Task<IActionResult> GetYourUpcomingActivities([FromQuery] int limit = 5)
    {
        var result = await Mediator.Send(new GetYourUpcomingActivities(limit));
        return Ok(result);
    }

    [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
    [HttpPut("pay-for-client-activity")]
    public async Task<IActionResult> PayForClientActivity([FromBody] PayForClientActivity request, CancellationToken cancellationToken)
    {
        try
        {
            await Mediator.Send(request, cancellationToken);
            return Ok(new { message = "Successfully paid for client activity." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InstanceOfActivityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ClientNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ClientWithdrawnException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ActivityAlreadyPaidException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ActivityCanceledException ex)
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
}


