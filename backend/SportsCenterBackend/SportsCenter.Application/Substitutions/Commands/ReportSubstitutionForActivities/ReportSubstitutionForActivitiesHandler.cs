using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Application.Exceptions.SubstitutionsExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Substitutions.Commands.ReportSubstitutionForActivities
{
    internal sealed class ReportSubstitutionForActivitiesHandler : IRequestHandler<ReportSubstitutionForActivities, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly ISubstitutionRepository _substitutionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportSubstitutionForActivitiesHandler(ISportActivityRepository sportActivityRepository, ISubstitutionRepository substitutionRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = sportActivityRepository;
            _substitutionRepository = substitutionRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(ReportSubstitutionForActivities request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to report need for activities.");
            }

            int absentEmployee = userId;

            var instanceOfActivity = await _sportActivityRepository.GetInstanceOfActivityAsync(request.ActivityId, request.ActivityDate, cancellationToken);

            if (instanceOfActivity == null)
            {
                throw new SportActivityNotFoundException(request.ActivityId);
            }

            if(instanceOfActivity.CzyOdwolane.HasValue && instanceOfActivity.CzyOdwolane.Value == true)
            {
                throw new ActivityCanceledException(request.ActivityId);
            }

            var alreadyRequested = await _substitutionRepository.HasEmployeeAlreadyRequestedSubstitutionAsync(request.ActivityId, userId);
            if (alreadyRequested)
            {
                throw new DuplicateSubstitutionActivityRequestException(request.ActivityId);
            }

            var isTrainerAssignedToActivity = await _sportActivityRepository.IsTrainerAssignedToActivityAsync(request.ActivityId, userId);

            if (!isTrainerAssignedToActivity)
            {
                throw new SubstitutionForActivitiesNotAllowedException(request.ActivityId);
            }

            var activitySchedule = await _sportActivityRepository.GetScheduleByActivityIdAsync(request.ActivityId, cancellationToken);

            var startHour = TimeOnly.FromTimeSpan(activitySchedule.GodzinaOd);
            var endHour = startHour.AddMinutes(activitySchedule.CzasTrwania);

            var substitution = new Zastepstwo
            {
                Data = instanceOfActivity.Data,
                GodzinaOd = startHour,
                GodzinaDo = endHour,
                ZajeciaId = request.ActivityId,
                RezerwacjaId = null,
                PracownikNieobecnyId = absentEmployee,
                PracownikZastepujacy = null,
                PracownikZatwierdzajacy = null
            };

            await _substitutionRepository.AddSubstitutionForActivitiesAsync(substitution);

            return Unit.Value;
        }
    }
}
