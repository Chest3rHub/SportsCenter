using MediatR;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.SportsCenterExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsClubManagement.Commands.AddSportsClubWorkingHours
{
    internal sealed class SetSportsCenterWorkingHoursHandler : IRequestHandler<SetSportsCenterWorkingHours, Unit>
    {
        private readonly ISportsCenterRepository _sportsCenterRepository;

        public SetSportsCenterWorkingHoursHandler(ISportsCenterRepository sportsCenterRepository)
        {
           _sportsCenterRepository = sportsCenterRepository;
        }

        public async Task<Unit> Handle(SetSportsCenterWorkingHours request, CancellationToken cancellationToken)
        {

            TimeOnly newOpenHour = TimeOnly.FromTimeSpan(request.OpenHour);
            TimeOnly newCloseHour = TimeOnly.FromTimeSpan(request.CloseHour);

            DayOfWeek targetDayOfWeek = ConvertStringToDayOfWeek(request.DayOfWeek);

            var conflictingReservations = await _sportsCenterRepository.GetConflictingReservationsOrActivitiesByDayOfWeek(targetDayOfWeek, newOpenHour, newCloseHour, cancellationToken);

            // w przyszlosci to zostanie zwrocone na frontend
            if (conflictingReservations.Any())
            {
                Console.WriteLine($"Istnieją rezerwacje lub zajęcia kolidujące z nowymi godzinami pracy:{newOpenHour}, {newCloseHour}");

                foreach (var conflict in conflictingReservations)
                {
                    if (conflict is Rezerwacja reservation)
                    {
                        Console.WriteLine($"Rezerwacja ID: {reservation.RezerwacjaId}");
                    }
                    else if (conflict is InstancjaZajec activityInstance)
                    {
                        Console.WriteLine($"Zajęcia ID: {activityInstance.GrafikZajec.ZajeciaId}, {activityInstance.Data}");
                    }
                    else
                    {
                        Console.WriteLine($"Nieoczekiwany typ obiektu: {conflict.GetType().Name}");
                    }
                }

                var conflicts = conflictingReservations.Select(conflict => 
                {
                    if (conflict is Rezerwacja reservation)
                    {
                        return new ConflictInfo("Rezerwacja", reservation.RezerwacjaId);
                    }
                    else if (conflict is InstancjaZajec activityInstance)
                    {
                        return new ConflictInfo("Zajęcia", activityInstance.GrafikZajec.ZajeciaId);
                    }
                    return null;
                }).Where(x => x != null).ToList();

                throw new ConflictException(conflicts);
            }

            var dayOfWeekExists = await _sportsCenterRepository.CheckIfDayExistsAsync(request.DayOfWeek, cancellationToken);

            if (!dayOfWeekExists)
            {
                throw new DayOfWeekNotFoundException(request.DayOfWeek);
            }

            var existingWorkingHours = await _sportsCenterRepository.GetWorkingHoursByDayAsync(request.DayOfWeek, cancellationToken);

            if (existingWorkingHours != null)
            {

                existingWorkingHours.GodzinaOtwarcia = TimeOnly.FromTimeSpan(request.OpenHour);
                existingWorkingHours.GodzinaZamkniecia = TimeOnly.FromTimeSpan(request.CloseHour);

                await _sportsCenterRepository.UpdateWorkingHours(existingWorkingHours, cancellationToken);
            }
            else
            {
                var newWorkingHours = new GodzinyPracyKlubu
                {
                    GodzinaOtwarcia = TimeOnly.FromTimeSpan(request.OpenHour),
                    GodzinaZamkniecia = TimeOnly.FromTimeSpan(request.CloseHour),
                    DzienTygodnia = request.DayOfWeek
                };

                await _sportsCenterRepository.AddWorkingHoursForGivenDay(newWorkingHours, cancellationToken);
            }

            return Unit.Value;
        }
        public static DayOfWeek ConvertStringToDayOfWeek(string dayOfWeekString)
        {
            var dayOfWeekMapping = new Dictionary<string, DayOfWeek>
            {
                { "poniedzialek", DayOfWeek.Monday },
                { "wtorek", DayOfWeek.Tuesday },
                { "sroda", DayOfWeek.Wednesday },
                { "czwartek", DayOfWeek.Thursday },
                { "piatek", DayOfWeek.Friday },
                { "sobota", DayOfWeek.Saturday },
                { "niedziela", DayOfWeek.Sunday }
            };

            if (dayOfWeekMapping.TryGetValue(dayOfWeekString.ToLower(), out var dayOfWeek))
            {
                return dayOfWeek;
            }
            else
            {
                throw new ArgumentException($"Invalid day of the week: {dayOfWeekString}");
            }
        }

    }
}
