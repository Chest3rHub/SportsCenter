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
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignUpForActivityHandler(ISportActivityRepository SportActivityRepository, IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = SportActivityRepository;
            _reservationRepository = reservationRepository;
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

            var (signedCount, limit) = await _sportActivityRepository.GetSignedUpClientCountAsync(request.ActivityId, request.SelectedDate, cancellationToken);

            if (limit.HasValue && signedCount >= limit.Value)
            {
                throw new LimitOfPlacesReachedException();
            }

            var activityHour = scheduleActivity.GodzinaOd;
         
            var activityDate = new DateTime(request.SelectedDate.Year, request.SelectedDate.Month, request.SelectedDate.Day, activityHour.Hours, activityHour.Minutes, 0);
            Console.WriteLine("AAAAAAAAAAAA activity date: " + activityDate);

            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);

            var timeDifference = activityDate - localNow;
            Console.WriteLine("AAAAAAAAAAAA localNow: " + localNow);
            Console.WriteLine("AAAAAAAAAAAA time diff: " + timeDifference);

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

            if (instanceOfActivity.CzyOdwolane.HasValue && instanceOfActivity.CzyOdwolane.Value == true)
            {
                throw new ActivityCanceledException(instanceOfActivity.GrafikZajec.ZajeciaId);
            }

            var iaAlreagdySignedUp = await _sportActivityRepository.IsClientSignedUpAsync(clientId, instanceOfActivity.InstancjaZajecId, cancellationToken);
            if (iaAlreagdySignedUp)
            {
                throw new ClientAlreadySignedUpException(clientId);
            }

            //czy w tym czasie klient jest zapisany na inne zaj lub ma zlozona rezerwacje
            var isAvailable = await _sportActivityRepository.IsClientAvailableForActivityAsync(clientId, request.ActivityId, request.SelectedDate, cancellationToken);

            if (!isAvailable)
            {
                throw new ClientAlreadyHasActivityOrReservationException();
            }

            var zapis = new InstancjaZajecKlient
            {
                KlientId = clientId,
                DataZapisu = DateOnly.FromDateTime(DateTime.UtcNow),
                DataWypisu = null,
                CzyUwzglednicSprzet = request.IsEquipmentIncluded ? true : false,
                InstancjaZajecId = instanceOfActivity.InstancjaZajecId,
                CzyOplacone = false,
                CzyZwroconoPieniadze = false
            };

            await _sportActivityRepository.AddClientToInstanceAsync(zapis, cancellationToken);

            return Unit.Value;
        }

    }
}
