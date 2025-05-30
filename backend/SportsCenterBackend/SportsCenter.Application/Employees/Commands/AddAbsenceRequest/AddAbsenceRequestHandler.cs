﻿using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

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

            int startHourInMinutes = (int)startDateTime.TimeOfDay.TotalMinutes;
            int endHourInMinutes = (int)endDateTime.TimeOfDay.TotalMinutes;
       
            var isTrainerFree = await _employeeRepository.IsTrainerAvailableAsync(userId,startDateTime,startHourInMinutes,endHourInMinutes,cancellationToken);
            
            if (isTrainerFree == TrainerAvailabilityStatus.HasReservations)
            {
                throw new CantAddAbsenceRequestException();
            }
            if (isTrainerFree == TrainerAvailabilityStatus.HasActivities)
            {
                throw new CantAddAbsenceRequestException();
            }
            if (isTrainerFree == TrainerAvailabilityStatus.IsUnavailable)
            {
                throw new CantAddAbsenceRequestForNoAvailabilityDayException();
            }

            var existingAbsenceRequest = await _employeeRepository.GetAbsenceRequestAsync(userId, request.Date, cancellationToken);

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
