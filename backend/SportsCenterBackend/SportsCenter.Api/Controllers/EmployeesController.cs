using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Employees.Commands.RegisterEmployee;
using FluentValidation;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Users.Queries.GetClients;
using SportsCenter.Application.Employees.Queries;
using SportsCenter.Application.Employees.Commands.AddTask;
using System.Threading.Tasks;
using SportsCenter.Application.Exceptions.TaskExceptions;
using SportsCenter.Application.Employees.Commands.RemoveTask;
using SportsCenter.Application.Employees.Queries.GetEmployees;
using SportsCenter.Application.Employees.Queries.GetTasks;
using SportsCenter.Application.Users.Commands.RegisterClient;


namespace SportsCenter.Api.Controllers
{
    [Route("[controller]")]
    public class EmployeesController : BaseController
    {
        public EmployeesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> ShowEmployeeAsync()
        {
            return Ok(await Mediator.Send(new GetEmployees()));
        }

        [AllowAnonymous]
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
                catch(WrongPositionNameException ex)
                {
                    return Conflict(new { message = ex.Message });
            }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Wystąpił błąd podczas wysyłania żądania", details = ex.Message });
                }
            }

        [HttpPost("Add-task")]
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
                return StatusCode(500, new { message = "Wystąpił błąd podczas wysyłania żądania", details = ex.Message });

            }
        }

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
                return StatusCode(500, new { message = "Wystąpił problem z usunięciem zadania.", error = ex.Message });
            }
        }

        [HttpGet("{pracownikId}/tasks")]
        public async Task<IActionResult> GetTasksForEmployee(int pracownikId, CancellationToken cancellationToken)
        {     
            var tasks = await Mediator.Send(new GetTasks(pracownikId), cancellationToken);

            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found for the given employee.");
            }
            return Ok(tasks);  
        }
    }
}