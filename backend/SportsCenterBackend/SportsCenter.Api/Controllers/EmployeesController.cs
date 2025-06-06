﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Employees.Commands.RegisterEmployee;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Employees.Commands.AddTask;
using SportsCenter.Application.Exceptions.TaskExceptions;
using SportsCenter.Application.Employees.Commands.RemoveTask;
using SportsCenter.Application.Employees.Queries.GetEmployees;
using SportsCenter.Application.Employees.Queries.GetTasks;
using SportsCenter.Application.Employees.Commands.EditTask;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Employees.Commands.DismissEmployee;
using System.Security.Claims;
using SportsCenter.Application.Employees.Commands.AddAdmTask;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Employees.Commands.AddTrainerCertificate;
using SportsCenter.Application.Employees.Commands.DeleteTrainerCertificate;
using SportsCenter.Application.Employees.Commands.UpdateTrainerCertificate;
using SportsCenter.Application.Employees.Queries.GetTrainerCertificates;
using SportsCenter.Application.Employees.Queries.GetYourCertificates;
using SportsCenter.Application.Employees.Commands.AddAbsenceRequest;
using SportsCenter.Application.Employees.Queries.GetAbsenceRequest;
using SportsCenter.Application.Employees.Commands.AcceptAbsenceRequest;
using SportsCenter.Application.Employees.Queries.GetYourAbsenceRequests;
using SportsCenter.Application.Employees.Queries.GetTrainerSchedule;
using SportsCenter.Application.Employees.Queries.GetTrainers;
using SportsCenter.Application.Employees.Queries.GetAvailableTrainers;
using SportsCenter.Application.Employees.Queries.GetEmployeeTasksByOwner;
using SportsCenter.Application.Employees.Queries.GetTrainerBusyTimes;
using SportsCenter.Application.Employees.Queries.GetEmployeesPositions;


namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class EmployeesController : BaseController
    {
        public EmployeesController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpGet]
        public async Task<IActionResult> ShowEmployeeAsync([FromQuery] int offset = 0)
        {
            return Ok(await Mediator.Send(new GetEmployees(offset)));
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpGet("get-employees-positions")]
        public async Task<IActionResult> GetEmployeesPositionsAsync()
        {
            return Ok(await Mediator.Send(new GetEmployeesPositions()));
        }

        [Authorize(Roles = "Wlasciciel,Klient,Pracownik administracyjny")]
        [HttpGet("get-trainers")]
        public async Task<IActionResult> GetTrainersAsync()
        {
            return Ok(await Mediator.Send(new GetTrainers()));
        }


        [Authorize(Roles = "Wlasciciel,Klient,Pracownik administracyjny")]
        [HttpGet("get-available-trainers")]
        public async Task<IActionResult> GetAvailableTrainersAsync([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {    
            var availableTrainers = await Mediator.Send(new GetAvailableTrainers(startTime, endTime));
            return Ok(availableTrainers);
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterEmployeesAsync([FromBody] RegisterEmployee registerEmployee)
        {
            var validationResults = new RegisterEmployeeValidator().Validate(registerEmployee);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }

            try
            {
                await Mediator.Send(registerEmployee);
                return NoContent();
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (WrongPositionNameException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [HttpDelete("dismiss-employee")]
        [Authorize(Roles = "Wlasciciel")]
        public async Task<IActionResult> DismissEmployee([FromBody] DismissEmployee dismissEmployee)
        {
            try
            {
                var response = await Mediator.Send(new DismissEmployee(dismissEmployee.DismissedEmployeeId, dismissEmployee.DismissalDate));

                if (response.FailedReservationIds.Any())
                {
                    return BadRequest(new
                    {
                        message = "Some reservations are removed.",
                        failedReservations = response.FailedReservationIds
                    });
                }

                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (EmployeeAlreadyDismissedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        //dodawanie zadan samemu sobie
        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpPost("Self-Add-task")]
        public async Task<IActionResult> AddTask([FromBody] AddTask task)
        {
            try
            {
                await Mediator.Send(task);
                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        //dodawanie zadan pracownikowi administracyjnemu
        [Authorize(Roles = "Wlasciciel")]
        [HttpPost("Add-task")]
        public async Task<IActionResult> AddTaskForAdmEmployee([FromBody] AddAdmTask task)
        {
            try
            {
                await Mediator.Send(task);
                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (NotAdmEmployeeException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpDelete("Delete-task")]
        public async Task<IActionResult> DeleteTaskAsync(RemoveTask task, CancellationToken cancellationToken)
        {
            try
            {
                await Mediator.Send(task);
                return NoContent();
            }
            catch (TaskNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpGet("get-your-tasks")]
        public async Task<IActionResult> GetTasksForEmployee([FromQuery] int offset = 0)
        {
            try
            {
                var tasks = await Mediator.Send(new GetTasks(offset));

                if (tasks == null || !tasks.Any())
                {
                    return NotFound("No tasks found for the given employee");
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpPut("Edit-task")]
        public async Task<IActionResult> UpdateTask(EditTask task, CancellationToken cancellationToken)
        {
            try
            {
                await Mediator.Send(task);
                return NoContent();
            }
            catch (TaskNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Pracownik administracyjny,Wlasciciel")]
        [HttpPost("Add-trainer-certificate")]
        public async Task<IActionResult> AddTrainerCertificate([FromBody] AddTrainerCertificate trainerCertificate)
        {
            try
            {
                await Mediator.Send(trainerCertificate);
                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (NotTrainerEmployeeException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpDelete("Remove-trainer-certificate")]
        public async Task<IActionResult> RemoveTrainerCertificate([FromBody] RemoveTrainerCertificate trainerCertificate)
        {
            try
            {
                await Mediator.Send(trainerCertificate);
                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (NotTrainerEmployeeException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (TrainerCertificateNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpPut("Update-trainer-certificate")]
        public async Task<IActionResult> UpdateTrainerCertificate([FromBody] UpdateTrainerCertificate trainerCertificate)
        {
            try
            {
                await Mediator.Send(trainerCertificate);
                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (NotTrainerEmployeeException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (TrainerCertificateNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpGet("Get-trainer-certificates")]
        public async Task<IActionResult> GetTrainerCertificates(int trainerId, [FromQuery] int offset = 0)
        {
            try
            {
                return Ok(await Mediator.Send(new GetTrainerCertificates(trainerId, offset)));
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (NotTrainerEmployeeException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Trener")]
        [HttpGet("Get-your-certificates")]
        public async Task<IActionResult> GetYourCertificates([FromQuery] int offset = 0)
        {
            return Ok(await Mediator.Send(new GetYourCertificates(offset)));
        }

        [Authorize(Roles = "Trener")]
        [HttpPost("add-absence-request")]
        public async Task<IActionResult> AddAbsenceRequest([FromBody] AddAbsenceRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(request));
            }
            catch(CantAddAbsenceRequestException ex)
            {
                return Conflict(new { message = ex.Message });
            }catch (CantAddAbsenceRequestForNoAvailabilityDayException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
        [HttpGet("get-absence-requests")]
        public async Task<IActionResult> GetAbsenceRequests([FromQuery] int offset = 0)
        {
            return Ok(await Mediator.Send(new GetAbsenceRequests(offset)));
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny")]
        [HttpPut("accept-absence-request")]
        public async Task<IActionResult> AcceptAbsenceRequest([FromBody] AcceptAbsenceRequest request)
        {
            try
            {
                return Ok(await Mediator.Send(request));
            }
            catch (AbsenceRequestNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (AbsenceRequestAlreadyAcceptedException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Trener")]
        [HttpGet("get-your-absence-requests")]
        public async Task<IActionResult> GetYourAbsenceRequests([FromQuery] int offset = 0)
        {
            return Ok(await Mediator.Send(new GetYourAbsenceRequests(offset)));
        }

        [Authorize(Roles = "Trener")]
        [HttpGet("get-your-schedule")]
        public async Task<IActionResult> GetYourSchedule([FromQuery] int offset = 0)
        {
            return Ok(await Mediator.Send(new GetTrainerSchedule(offset)));
        }

        [Authorize(Roles = "Wlasciciel")]
        [HttpGet("get-employee-tasks-by-owner")]
        public async Task<IActionResult> GetEmployeeTasksByOwner([FromQuery] int employeeId)
        {
            try
            {
                var tasks = await Mediator.Send(new GetEmployeeTasksByOwner(employeeId));

                if (tasks == null || !tasks.Any())
                {
                    return NotFound("No tasks found assigned by owner for the given emp");
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        [Authorize(Roles = "Wlasciciel,Pracownik administracyjny,Klient")]
        [HttpGet("{trainerId}/busy-times")]
        public async Task<IActionResult> GetTrainerBusyTimes(int trainerId, [FromQuery] DateTime date, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetTrainerBusySlots(trainerId, date)));
        }
    }
}