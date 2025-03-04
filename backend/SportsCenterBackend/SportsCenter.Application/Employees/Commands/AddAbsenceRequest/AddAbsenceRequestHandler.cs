using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddAbsenceRequest
{
    internal sealed class AddAbsenceRequestHandler : IRequestHandler<AddAbsenceRequest, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddAbsenceRequestHandler(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(AddAbsenceRequest request, CancellationToken cancellationToken)
        { 
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to add absence request.");
            }

            var startDateTime = request.Date.ToDateTime(TimeOnly.FromTimeSpan(request.StartHour));
            var endDateTime = request.Date.ToDateTime(TimeOnly.FromTimeSpan(request.EndHour));

            var isTrainerFree = await _employeeRepository.IsTrainerAvailableAsync(userId, startDateTime, endDateTime, cancellationToken);

            if (!isTrainerFree)
            {
                throw new CantAddAbsenceRequestException();
            }

            var existingAbsenceRequest = await _employeeRepository.GetAbsenceRequestAsync(userId, request.Date);

            if (existingAbsenceRequest != null)
            {
                existingAbsenceRequest.GodzinaOd = TimeOnly.FromTimeSpan(request.StartHour);
                existingAbsenceRequest.GodzinaDo = TimeOnly.FromTimeSpan(request.EndHour);
                existingAbsenceRequest.CzyZatwierdzone = false;

                await _employeeRepository.UpdateAbsenceRequestAsync(existingAbsenceRequest, cancellationToken);
            }
            else
            {
                var absenceRequest = new BrakDostepnosci
                {
                    Data = request.Date,
                    GodzinaOd = TimeOnly.FromTimeSpan(request.StartHour),
                    GodzinaDo = TimeOnly.FromTimeSpan(request.EndHour),
                    CzyZatwierdzone = false,
                    PracownikId = userId
                };

                await _employeeRepository.AddAbsenceRequestAsync(absenceRequest, cancellationToken);
            }
            return Unit.Value;
        }
    }
}
