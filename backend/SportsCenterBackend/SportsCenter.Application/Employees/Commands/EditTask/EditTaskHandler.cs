using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.TaskExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.EditTask
{
    internal class EditTaskHandler : IRequestHandler<EditTask, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditTaskHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(EditTask request, CancellationToken cancellationToken)
        {
            
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to edit task.");
            }

            var existingTask = await _employeeRepository.GetTaskByIdAsync(request.ZadanieId, cancellationToken);

            if (existingTask == null)
            {
                throw new TaskNotFoundException(request.ZadanieId);
            }

            if (existingTask.PracownikZlecajacyId != userId)
            {
                throw new UnauthorizedAccessException("You can only edit your own tasks.");
            }

            existingTask.Opis = request.Opis;
            existingTask.DataDo = DateOnly.FromDateTime(request.DataDo);

            await _employeeRepository.UpdateTaskAsync(existingTask, cancellationToken);

            return Unit.Value;
        }
    }
}
