using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


//klasa do przypadku uzycia "dodawanie zadan to-do w swojej liscie"

namespace SportsCenter.Application.Employees.Commands.AddTask
{
    internal sealed class AddTaskHandler : IRequestHandler<AddTask, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddTaskHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddTask request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permissions to add tasks.");
            }

            var pracownik = await _employeeRepository.GetEmployeeByIdAsync(userId, cancellationToken);
            if (pracownik == null)
            {
                throw new EmployeeNotFoundException(userId);
            }

            var newTask = new Zadanie
            {
                Opis = request.Description,
                DataDo = DateOnly.FromDateTime(request.DateTo),
                PracownikId = userId,         
                PracownikZlecajacyId = userId 
            };

            await _employeeRepository.AddTaskAsync(newTask, cancellationToken);

            return Unit.Value;
        }
    }
}
