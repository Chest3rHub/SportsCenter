﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Clients.Commands.RegisterClient;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Users.Commands.ChangePassowrd;
using SportsCenter.Application.Users.Commands.ChangePassword;
using SportsCenter.Application.Users.Commands.ChangeUserPassword;
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

        [Authorize(Roles = "Wlasciciel,Klient,Pomoc sprzatajaca,Trener,Pracownik administracyjny")]
        [HttpPost("change-password-yourself")]
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
        [HttpPost("change-other-user-password")]
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
    }
}
