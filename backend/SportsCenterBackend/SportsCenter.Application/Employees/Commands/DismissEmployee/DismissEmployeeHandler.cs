using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SportsCenter.Application.Employees.Commands.DismissEmployee
{
    internal sealed class DismissEmployeeHandler : IRequestHandler<DismissEmployee, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DismissEmployeeHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(DismissEmployee request, CancellationToken cancellationToken)
        {      
            //var user = _httpContextAccessor.HttpContext?.User;
            //var role = user?.FindFirst(ClaimTypes.Role)?.Value;

            //if (role != "Owner")
            //{
            //    throw new UnauthorizedAccessException("Only owner can dismiss employee.");
            //}

            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId, cancellationToken);
            if (employee == null)
            {
                throw new EmployeeNotFoundException(request.EmployeeId);
            }

            await _employeeRepository.DeleteEmployeeAsync(employee, cancellationToken);

            return Unit.Value;
        }
    }
}
