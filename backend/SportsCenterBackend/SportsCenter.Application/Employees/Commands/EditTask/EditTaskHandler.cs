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
            //tu bedzie sprawdzenie czy id osoby z sesji jest takie jak id zlecajacego
            //zadanie i tylko wtedy bedzie mozna je edytowac inaczej blad

            var existingTask = await _employeeRepository.GetTaskByIdAsync(request.ZadanieId, cancellationToken);

            if (existingTask == null)
            {
                throw new TaskNotFoundException(request.ZadanieId);
            }

            existingTask.Opis = request.Opis;
            existingTask.DataDo = DateOnly.FromDateTime(request.DataDo);

            await _employeeRepository.UpdateTaskAsync(existingTask, cancellationToken);

            return Unit.Value;
        }
    }
}
