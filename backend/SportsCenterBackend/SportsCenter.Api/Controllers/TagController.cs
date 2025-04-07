using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.TagsException;
using SportsCenter.Application.Tags.Commands.AddTag;
using SportsCenter.Application.Tags.Commands.RemoveTag;
using SportsCenter.Application.Tags.Queries.GetTags;

namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class TagController : BaseController
    {
        public TagController(IMediator mediator) : base(mediator)
        {

        }

        [Authorize(Roles = "Wlasciciel, Pracownik administracyjny")]
        [HttpGet]
        public async Task<IActionResult> GetTagsAsync([FromQuery] int offset = 0)
        {
            return Ok(await Mediator.Send(new GetTags(offset)));
        }

        [Authorize(Roles = "Wlasciciel, Pracownik administracyjny")]
        [HttpPost("add-tag")]
        public async Task<IActionResult> AddTag([FromBody] AddTag request)
        {
            try
            {
                return Ok(await Mediator.Send(request));
            }
            catch (TagAlreadyExistException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
        [HttpDelete("Remove-tag")]
        public async Task<IActionResult> RemoveTag([FromBody] RemoveTag request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (TagNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }
    }
}
