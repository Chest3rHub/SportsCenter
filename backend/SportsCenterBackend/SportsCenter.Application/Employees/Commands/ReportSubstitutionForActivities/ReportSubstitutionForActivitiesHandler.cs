using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.ReportSubstitutionNeed
{
    internal sealed class ReportSubstitutionForActivitiesHandler : IRequestHandler<ReportSubstitutionForActivities, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportSubstitutionForActivitiesHandler(ISportActivityRepository sportActivityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = sportActivityRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(ReportSubstitutionForActivities request, CancellationToken cancellationToken)
        {

            var (date, duration) = await _sportActivityRepository.GetActivityDetailsByIdAsync(request.ActivitiesId);

            if (!date.HasValue || !duration.HasValue)
            {
                throw new SportActivityNotFoundException(request.ActivitiesId);
            }

            DateTime startHour = date.Value;
            DateTime endHour = startHour.AddMinutes(duration.Value);

            TimeOnly startHourTimeOnly = TimeOnly.FromDateTime(startHour);
            TimeOnly endHourTimeOnly = TimeOnly.FromDateTime(endHour);

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to report need for activities.");
            }
            int absentEmployee = userId;

            var isTrainerAssignedToActivity = await _sportActivityRepository.IsTrainerAssignedToActivityAsync(request.ActivitiesId, userId);

            if (!isTrainerAssignedToActivity)
            {
                throw new SubstitutionForActivitiesNotAllowedException(request.ActivitiesId);
            }

            var alreadyRequested = await _sportActivityRepository.HasEmployeeAlreadyRequestedSubstitutionAsync(request.ActivitiesId, userId);
            if (alreadyRequested)
            {
                throw new DuplicateSubstitutionActivityRequestException(request.ActivitiesId);
            }

            var substitution = new Zastepstwo
            {
                Data = DateOnly.FromDateTime(date.Value),
                GodzinaOd = startHourTimeOnly,
                GodzinaDo = endHourTimeOnly,
                ZajeciaId = request.ActivitiesId,
                RezerwacjaId = null,
                PracownikNieobecnyId = absentEmployee,
                PracownikZastepujacy = null,
                PracownikZatwierdzajacy = null
            };

            await _sportActivityRepository.AddSubstitutionForActivitiesAsync(substitution);

            return Unit.Value;
        }
    }
}
