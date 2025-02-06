using MediatR;
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
                    return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
                }
            }

        [HttpDelete("{employeeId}")]
        //[Authorize(Roles = "Owner")]
        public async Task<IActionResult> DismissEmployee(int employeeId)
        {
            try
            {
                await Mediator.Send(new DismissEmployee(employeeId));
                return NoContent();
            }
            catch (EmployeeNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            //catch (UnauthorizedAccessException)
            //{
            //    return Forbid();
            //}
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });
            }
        }

        //dodawanie zadan samemu sobie
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

        //dodawanie zadan pracownikowi administracyjunemu
        [HttpPost("Add-task")]
        public async Task<IActionResult> AddTaskForAdmEmployee([FromBody] AddTask task)
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
            catch(NotAdmEmployeeException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while sending the request", details = ex.Message });

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
                return StatusCode(500, new { message = "An error occurred while removing the task", error = ex.Message });
            }
        }

        [HttpGet("{pracownikId}/tasks")]
        public async Task<IActionResult> GetTasksForEmployee(int pracownikId, CancellationToken cancellationToken)
        {     
            var tasks = await Mediator.Send(new GetTasks(pracownikId), cancellationToken);

            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found for the given employee");
            }
            return Ok(tasks);  
        }

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
            }catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the task", error = ex.Message });
            }
        }
    }
}