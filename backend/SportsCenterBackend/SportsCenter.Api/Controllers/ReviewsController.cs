using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.ReviewsExceptions;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Application.Reservations.Queries.GetReservationSummary;
using SportsCenter.Application.Reviews.Commands.AddReview;
using SportsCenter.Application.Reviews.Queries;
using System.Security.Claims;

namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class ReviewsController : BaseController
    {
        public ReviewsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("Add-review")]
        public async Task<IActionResult> AddReview([FromBody] AddReview addReview)
        {
            var validationResults = new AddReviewValidator().Validate(addReview);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }          
            try
            {              
                await Mediator.Send(addReview);
                return Ok(new { Message = "Review added successfully" });
            }
            catch (ReviewTimeExceededException)
            {
                return BadRequest("the time for submitting a rating has passed.");
            }
            catch (ReviewAlreadyExistException)
            {               
                return BadRequest("The clien has already has already rated this activity.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("Get-reviews")]
        public async Task<IActionResult> GetReviewsSummary([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)//nie jestem pewna gdzie ten warunek umiescic
            {
                return BadRequest("StartDate cannot be greater than EndDate.");
            }

            var query = new GetReviewsSummary(startDate, endDate);
            var result = await Mediator.Send(query);

            return Ok(result);         
        }
    }
}
