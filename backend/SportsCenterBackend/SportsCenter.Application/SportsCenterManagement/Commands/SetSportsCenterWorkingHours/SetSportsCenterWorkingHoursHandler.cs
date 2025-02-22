using MediatR;
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

            var dayOfWeekExists = await _sportsCenterRepository.CheckIfDayExistsAsync(request.DayOfWeekId, cancellationToken);

            if (!dayOfWeekExists)
            {
                throw new DayOfWeekNotFoundException(request.DayOfWeekId);
            }

            var existingWorkingHours = await _sportsCenterRepository.GetWorkingHoursByDayAsync(request.DayOfWeekId, cancellationToken);

            if (existingWorkingHours != null)
            {            
                existingWorkingHours.GodzinaOtwarcia = request.OpenHour;
                existingWorkingHours.GodzinaZamkniecia = request.CloseHour;

                await _sportsCenterRepository.UpdateWorkingHours(existingWorkingHours, cancellationToken);
            }
            else
            {
                var newWorkingHours = new GodzinyPracyKlubu
                {
                    DzienTygodniaId = request.DayOfWeekId,
                    GodzinaOtwarcia = request.OpenHour,
                    GodzinaZamkniecia = request.CloseHour
                };

                await _sportsCenterRepository.AddWorkingHoursForGivenDay(newWorkingHours, cancellationToken);
            }

            return Unit.Value;
        }
    }
}
