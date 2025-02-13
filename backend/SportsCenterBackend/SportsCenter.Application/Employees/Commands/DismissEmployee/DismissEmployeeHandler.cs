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
        private readonly IReservationRepository _reservationRepository; 
        private readonly ISportActivityRepository _sportActivityRepository; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DismissEmployeeHandler(IEmployeeRepository employeeRepository, IReservationRepository reservationRepository,
             ISportActivityRepository sportActivityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _reservationRepository = reservationRepository;
            _sportActivityRepository = sportActivityRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(DismissEmployee request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.EmployeeId, cancellationToken);
            if (employee == null)
            {
                throw new EmployeeNotFoundException(request.EmployeeId);
            }

            var reservations = await _reservationRepository.GetReservationsByTrainerIdAsync(request.EmployeeId, cancellationToken);
            if (reservations.Any())
            {
                throw new InvalidOperationException($"Pracownik o ID {request.EmployeeId} ma przypisane rezerwacje. Nie można go zwolnić.");
            }

            var activitySchedules = await _sportActivityRepository.GetSchedulesByTrainerIdAsync(request.EmployeeId, cancellationToken);
            if (activitySchedules.Any())
            {
                throw new InvalidOperationException($"Pracownik o ID {request.EmployeeId} ma przypisane zajęcia w grafiku. Nie można go zwolnić.");
            }
            await _employeeRepository.DeleteEmployeeAsync(employee, cancellationToken);

            return Unit.Value;
        }
    }
}
