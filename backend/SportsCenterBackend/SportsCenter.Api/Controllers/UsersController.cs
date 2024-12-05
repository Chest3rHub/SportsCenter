using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Users.Commands.ChangePassowrd;
using SportsCenter.Application.Users.Commands.Login;

namespace SportsCenter.Api.Controllers
{

    [Route("[controller]")]
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] Login loginCommand)
        {
            try
            {
                var loginResponse = await Mediator.Send(loginCommand);
                return Ok(loginResponse);
            }
            catch (InvalidLoginException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePassword changePassword)
        {              
            try
            {
                var result = await Mediator.Send(changePassword);
                return Ok(result);               

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }
    }
}
