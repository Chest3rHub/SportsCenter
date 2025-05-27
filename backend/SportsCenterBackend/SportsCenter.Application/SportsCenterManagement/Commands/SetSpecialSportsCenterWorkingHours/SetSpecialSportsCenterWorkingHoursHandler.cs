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

namespace SportsCenter.Application.SportsCenterManagement.Commands.SetSpecialSportsCenterWorkingHours
{
    internal sealed class SetSpecialSportsCenterWorkingHoursHandler : IRequestHandler<SetSpecialSportsCenterWorkingHours, Unit>
    {
        private readonly ISportsCenterRepository _sportsCenterRepository;

        public SetSpecialSportsCenterWorkingHoursHandler(ISportsCenterRepository sportsCenterRepository)
        {
            _sportsCenterRepository = sportsCenterRepository;
        }

        public async Task<Unit> Handle(SetSpecialSportsCenterWorkingHours request, CancellationToken cancellationToken)
        {
            var existingDate = await _sportsCenterRepository.GetSpecialWorkingHoursByDateAsync(request.Date, cancellationToken);

            TimeOnly newOpenHour = TimeOnly.FromTimeSpan(request.OpenHour);
            TimeOnly newCloseHour = TimeOnly.FromTimeSpan(request.CloseHour);

            var conflictingReservations = await _sportsCenterRepository.GetConflictingReservationsOrActivities(DateOnly.FromDateTime(request.Date), newOpenHour, newCloseHour, cancellationToken);

            //w przyszlosci to zostanie wyslane na frontend
            if (conflictingReservations.Any())
            {
                Console.WriteLine("Istnieją rezerwacje lub zajęcia kolidujące z nowymi godzinami pracy:");
                
                foreach (var conflict in conflictingReservations)
                {
                    if (conflict is Rezerwacja reservation)
                    {
                        Console.WriteLine($"Rezerwacja ID: {reservation.RezerwacjaId}");
                    }
                    else if (conflict is InstancjaZajec activityInstance)
                    {
                        Console.WriteLine($"Zajęcia ID: {activityInstance.GrafikZajec.ZajeciaId}");
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


            if (existingDate != null)
            {

                existingDate.GodzinaOtwarcia = TimeOnly.FromTimeSpan(request.OpenHour);
                existingDate.GodzinaZamkniecia = TimeOnly.FromTimeSpan(request.CloseHour);

                await _sportsCenterRepository.UpdateDateWorkingHours(existingDate, cancellationToken);
            }
            else
            {
                var newWorkingHours = new WyjatkoweGodzinyPracy
                {
                    Data = DateOnly.FromDateTime(request.Date),
                    GodzinaOtwarcia = TimeOnly.FromTimeSpan(request.OpenHour),
                    GodzinaZamkniecia = TimeOnly.FromTimeSpan(request.CloseHour)             
                };

                await _sportsCenterRepository.AddWorkingHoursForGivenDate(newWorkingHours, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
