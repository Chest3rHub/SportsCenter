using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Clients.Commands.RegisterClient;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Exceptions.UsersExceptions;
using SportsCenter.Application.Users.Commands.ChangePassowrd;
using SportsCenter.Application.Users.Commands.ChangePassword;
using SportsCenter.Application.Users.Commands.ChangeUserPassword;
using SportsCenter.Application.Users.Commands.Login;
using SportsCenter.Application.Users.Commands.RefreshToken;
using SportsCenter.Application.Users.Queries.GetUserAccountInfo;

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

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true, 
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1) // na godzine, moze w config gdzies to dac?
                };

                Response.Cookies.Append("accessToken", loginResponse.Token, cookieOptions);
                return Ok(new { role = loginResponse.Role });
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
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            try
            {
                var token = Request.Cookies["accessToken"];

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is missing from cookies." });
                }

                var refreshTokenRequest = new RefreshToken(token);
                var refreshTokenResponse = await Mediator.Send(refreshTokenRequest);

                
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                };

                Response.Cookies.Append("accessToken", refreshTokenResponse.Token, cookieOptions);

                return Ok();
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1) 
            };

            Response.Cookies.Append("accessToken", "", cookieOptions);

            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize(Roles = "Wlasciciel,Klient,Pomoc sprzatajaca,Trener,Pracownik administracyjny")]
        [HttpPut("change-password-yourself")]
        public async Task<IActionResult> ChangePasswordYourselfAsync([FromBody] ChangePasswordYourself changePassword)
        {
            var validationResults = new ChangePasswordYourselfValidator().Validate(changePassword);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

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

        [Authorize(Roles = "Wlasciciel")]
        [HttpPut("change-other-user-password")]
        public async Task<IActionResult> ChangeOtherUserPasswordAsync([FromBody] ChangeOtherUserPassword changePassword)
        {
            var validationResults = new ChangeOtherUserPasswordValidator().Validate(changePassword);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

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

        [Authorize]
        [HttpGet("account-info")]
        public async Task<IActionResult> GetUserAccountInfoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var userAccountInfo = await Mediator.Send(new GetUserAccountInfo(), cancellationToken);
                return Ok(userAccountInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }
    }
}
