using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Commands.SignUpForActivity
{
    internal class SignUpForActivityHandler : IRequestHandler<SignUpForActivity, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignUpForActivityHandler(ISportActivityRepository SportActivityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = SportActivityRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(SignUpForActivity request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new UnauthorizedAccessException("No user authorization.");
            }

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!int.TryParse(userIdClaim, out int clientId))
            {
                throw new UnauthorizedAccessException("Invalid user ID.");
            }

            if (!user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Klient"))
            {
                throw new UnauthorizedAccessException("Only customers can sign up for activities.");
            }

            //PO INTEGRACJI Z FRONTENDEM:
            //uzytkownik kliknie w kalendarzu kafelek instancjaZajec
            //frontend to zwroci i z tego obiektu na podstawie instancjaZajecId
            //znajde w bazie zajecia i juz cala dalsza logika bedzie sie zgadzała

            //wystarczy uzyc tej metody:
            //var activity = await _sportActivityRepository.GetActivityByInstanceOfActivityIdAsync(instanceOfActivityId, cancellationToken);
            var scheduleActivity = await _sportActivityRepository.GetScheduleByActivityIdAsync(request.ActivityId, cancellationToken);
            if (scheduleActivity == null)
            {
                throw new SportActivityNotFoundException(request.ActivityId);
            }

            var activityHour = scheduleActivity.GodzinaOd;
            var currentTime = DateTime.UtcNow;

            var activityDate = new DateTime(request.SelectedDate.Year, request.SelectedDate.Month, request.SelectedDate.Day, activityHour.Hours, activityHour.Minutes, 0);

            var timeDifference = activityDate - currentTime;

            if (timeDifference.TotalHours > 48)
            {
                throw new ActivityTimeTooFarException();
            }

            var daysOfWeekTranslation = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "poniedzialek" },
                { DayOfWeek.Tuesday, "wtorek" },
                { DayOfWeek.Wednesday, "sroda" },
                { DayOfWeek.Thursday, "czwartek" },
                { DayOfWeek.Friday, "piatek" },
                { DayOfWeek.Saturday, "sobota" },
                { DayOfWeek.Sunday, "niedziela" }
            };

            var startDayOfWeek = daysOfWeekTranslation[request.SelectedDate.DayOfWeek];

            if (startDayOfWeek != scheduleActivity.DzienTygodnia)
            {
                throw new InvalidDayOfWeekException(scheduleActivity.DzienTygodnia, request.SelectedDate);
            }

            var instanceOfActivity = await _sportActivityRepository.GetInstanceByScheduleAndDateAsync(scheduleActivity, request.SelectedDate, cancellationToken);

            if (instanceOfActivity == null)
            {
                instanceOfActivity = new InstancjaZajec
                {
                    Data = request.SelectedDate,
                    CzyOdwolane = false,
                    GrafikZajecId = scheduleActivity.GrafikZajecId       
                };
                await _sportActivityRepository.AddInstanceAsync(instanceOfActivity, cancellationToken);
            }

            var czyJuzZapisany = await _sportActivityRepository.IsClientSignedUpAsync(clientId, instanceOfActivity.InstancjaZajecId, cancellationToken);
            if (czyJuzZapisany)
            {
                throw new ClientAlreadySignedUpException(clientId);
            }

            var zapis = new InstancjaZajecKlient
            {
                KlientId = clientId,
                DataZapisu = DateOnly.FromDateTime(DateTime.UtcNow),
                DataWypisu = null,
                CzyUwzglednicSprzet = request.IsEquipmentIncluded,
                InstancjaZajecId = instanceOfActivity.InstancjaZajecId,
                CzyOplacone = false,
                CzyZwroconoPieniadze = false
            };

            await _sportActivityRepository.AddClientToInstanceAsync(zapis, cancellationToken);

            return Unit.Value;
        }
    }
}
