using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.NewsExceptions;
using SportsCenter.Application.News.Commands.AddNews;
using SportsCenter.Application.News.Commands.RemoveNews;
using SportsCenter.Application.News.Commands.UpdateNews;
using SportsCenter.Application.News.Queries.GetNews;

namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class NewsController : BaseController
    {
        public NewsController(IMediator mediator) : base(mediator)
        {
        }

        [AllowAnonymous]
        [HttpGet("Get-news")]
        public async Task<IActionResult> GetNewsAsync([FromQuery] int offset=0)
        {
            return Ok(await Mediator.Send(new GetNews(offset)));
        }

        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpPost("Add-news")]
        public async Task<IActionResult> AddNewsAsync([FromBody] AddNews addNews)
        {
            var validationResults = new AddNewsValidator().Validate(addNews);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }
            try
            {
                await Mediator.Send(addNews);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpPut("Update-news")]
        public async Task<IActionResult> UpdateNews([FromBody] UpdateNews updateNews)
        {
            var validationResults = new UpdateNewsValidator().Validate(updateNews);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

            try
            {
                await Mediator.Send(updateNews);
                return Ok(new { Message = "News updated successfully." });
            }
            catch (NewsNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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

        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpDelete("Remove-news")]
        public async Task<IActionResult> RemoveNews([FromQuery] int newsId)
        {
            try
            {
                await Mediator.Send(new RemoveNews(newsId));
                return Ok("News removed successfully");
            }
            catch (NewsNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }
    }
}
