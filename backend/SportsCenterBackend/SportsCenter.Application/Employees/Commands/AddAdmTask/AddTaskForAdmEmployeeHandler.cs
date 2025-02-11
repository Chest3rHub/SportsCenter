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

//klasa przeznaczona do realizacji przypadku "Dodawanie zadań to-do wybranemu pracownikowi administracyjnemu"

namespace SportsCenter.Application.Employees.Commands.AddAdmTask
{
    internal sealed class AddTaskForAdmEmployeeHandler : IRequestHandler<AddAdmTask, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddTaskForAdmEmployeeHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddAdmTask request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            var userRoleClaim = GetRoleFromClaims(_httpContextAccessor.HttpContext?.User);
      
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You do not have permission to add tasks.");
            }

            if (!IsOwnerRole(userRoleClaim))
            {
                throw new UnauthorizedAccessException("Only the owner can add tasks to administrative employees.");
            }

            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.PracownikId, cancellationToken);

            if (employee == null)
            {
                throw new EmployeeNotFoundException(request.PracownikId);
            }

            int admEmployeeId = (int)await _employeeRepository.GetEmployeeTypeByNameAsync("Pracownik administracyjny", cancellationToken);

            if (employee.IdTypPracownika != admEmployeeId)
            {
                throw new NotAdmEmployeeException(request.PracownikId);
            }

            var newTask = new Zadanie
            {
                Opis = request.Opis,
                DataDo = DateOnly.FromDateTime(request.DataDo),
                PracownikId = request.PracownikId,
                PracownikZlecajacyId = userId,
            };

            await _employeeRepository.AddTaskAsync(newTask, cancellationToken);

            return Unit.Value;
        }
        private string GetRoleFromClaims(ClaimsPrincipal user)
        {
            return user?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                   ?? user?.Claims.FirstOrDefault(c => c.Type == "role")?.Value
                   ?? user?.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
        }
        private bool IsOwnerRole(string userRole)
        {
            return userRole == "Wlasciciel";
        }
    }

}
