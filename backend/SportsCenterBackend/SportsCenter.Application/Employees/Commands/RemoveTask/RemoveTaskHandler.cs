using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.TaskExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.RemoveTask
{
    internal sealed class RemoveTaskHandler : IRequestHandler<RemoveTask, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveTaskHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(RemoveTask request, CancellationToken cancellationToken)
        {
            var task = await _employeeRepository.GetTaskByIdAsync(request.TaskId, cancellationToken);

            if (task == null)
            {
                throw new TaskNotFoundException(request.TaskId);
            }

            await _employeeRepository.RemoveTaskAsync(task, cancellationToken);

            return Unit.Value;
        }

    }
}
