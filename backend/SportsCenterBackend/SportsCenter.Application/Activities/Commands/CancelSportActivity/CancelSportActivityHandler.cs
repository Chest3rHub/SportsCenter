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

namespace SportsCenter.Application.Activities.Commands.CancelSportActivity
{
    internal class CancelSportActivityHandler : IRequestHandler<CancelSportActivity, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CancelSportActivityHandler(ISportActivityRepository SportActivityRepository, IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = SportActivityRepository;
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(CancelSportActivity request, CancellationToken cancellationToken)
        {

            var scheduleActivity = await _sportActivityRepository.GetScheduleByActivityIdAsync(request.SportActivityId, cancellationToken);
            if (scheduleActivity == null)
            {
                throw new SportActivityNotFoundException(request.SportActivityId);
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

            var startDayOfWeek = daysOfWeekTranslation[request.ActivityDate.DayOfWeek];

            if (startDayOfWeek != scheduleActivity.DzienTygodnia)
            {
                throw new InvalidActivityDateException(request.ActivityDate);
            }

            var InstanceOfActivity = await _sportActivityRepository.GetInstanceByScheduleAndDateAsync(scheduleActivity, request.ActivityDate, cancellationToken);

            if (InstanceOfActivity == null)
            {
                throw new SportActivityNotFoundException(request.SportActivityId);
            }

            int isAlreadyCanceled = await _sportActivityRepository.CancelInstanceOfActivityAsync(InstanceOfActivity.InstancjaZajecId, cancellationToken);
            if(isAlreadyCanceled == 0)
            {
                throw new ActivityAlreadyCanceledException();
            }
            var clients = await _clientRepository.GetClientsWhoPaidForCancelledActivitiesAsync(InstanceOfActivity.InstancjaZajecId, cancellationToken);

            foreach (var client in clients)
            {
                var refundAmount = await _clientRepository.CalculateRefundAmountAsync(client, InstanceOfActivity.InstancjaZajecId, cancellationToken);

                if (refundAmount > 0)
                {
                   int success = await _clientRepository.RefundClientAsync(client.KlientId, refundAmount, InstanceOfActivity.InstancjaZajecId, cancellationToken);
                   if(success == 0)
                    {
                        throw new RefundAlreadyGivenException(InstanceOfActivity.InstancjaZajecId, client.KlientId);
                    }
                }
            }
            return Unit.Value;
        }
    }
}
