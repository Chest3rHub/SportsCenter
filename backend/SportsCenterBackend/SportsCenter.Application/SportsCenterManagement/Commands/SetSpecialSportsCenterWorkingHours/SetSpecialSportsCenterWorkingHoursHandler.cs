using MediatR;
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
