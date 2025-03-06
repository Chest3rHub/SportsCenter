using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SubstitutionsExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportsCenter.Application.Substitutions.Commands.AssignSubstitution
{
    internal sealed class AssignSubstitutionHandler : IRequestHandler<AssignSubstitution, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISubstitutionRepository _substitutionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AssignSubstitutionHandler(ISportActivityRepository sportActivityRepository, ISubstitutionRepository substitutionRepository, IReservationRepository reservationRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = sportActivityRepository;
            _substitutionRepository = substitutionRepository;
            _reservationRepository = reservationRepository;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(AssignSubstitution request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to assign substitution.");
            }
            int approvedEmployeeId = userId;

            var trainerPosition = await _employeeRepository.GetEmployeePositionNameByIdAsync(request.SubstituteEmployeeId, cancellationToken);
            if (trainerPosition == null)
            {
                throw new EmployeeNotFoundException(request.SubstituteEmployeeId);
            }

            if (trainerPosition != "Trener")//musi byc Trener w bazie w tabeli TypPracownika
            {
                throw new NotTrainerEmployeeException(request.SubstituteEmployeeId);
            }

            var substitution = await _substitutionRepository.GetSubstitutionByIdAsync(request.SubstitutionId, cancellationToken);

            if (substitution == null)
            {
                throw new SubstitutionNotFoundException(request.SubstitutionId);
            }

            DateTime date;
            int startHourInMinutes;
            int endHourInMinutes;

            if (substitution.ZajeciaId.HasValue)
            {           
                var activityDetails = await _sportActivityRepository.GetActivityDetailsAsync(substitution.ZajeciaId.Value, cancellationToken);
                if (activityDetails == null)
                {
                    throw new SportActivityNotFoundException(substitution.ZajeciaId.Value);
                }

                date = activityDetails.Value.date;

                startHourInMinutes = (int)activityDetails.Value.startTime.TotalMinutes;
                endHourInMinutes = (int)activityDetails.Value.endTime.TotalMinutes;
            }
            else
            {
                var reservationDetails = await _reservationRepository.GetReservationDetailsAsync(substitution.RezerwacjaId.Value, cancellationToken);
                if (reservationDetails == null)
                {
                    throw new ReservationNotFoundException(substitution.RezerwacjaId.Value);
                }

                date = reservationDetails.Value.startDateTime;

                startHourInMinutes = (int)reservationDetails.Value.startDateTime.TimeOfDay.TotalMinutes;
                endHourInMinutes = (int)reservationDetails.Value.endDateTime.TimeOfDay.TotalMinutes;
            }

            var availabilityStatus = await _employeeRepository.IsTrainerAvailableAsync(request.SubstituteEmployeeId,date,startHourInMinutes,endHourInMinutes,cancellationToken);

            if (availabilityStatus == TrainerAvailabilityStatus.IsFired)
            {
                throw new EmployeeAlreadyDismissedException(request.SubstituteEmployeeId);
            }

            if (availabilityStatus != TrainerAvailabilityStatus.Available)
            {
                throw new TrainerNotAvaliableException();
            }

            await _substitutionRepository.UpdateSubstitutionAsync(request.SubstitutionId, request.SubstituteEmployeeId, approvedEmployeeId, cancellationToken);

            return Unit.Value;
        }
    }
}
