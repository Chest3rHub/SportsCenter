using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Enums;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;

namespace SportsCenter.Application.Activities.Commands.PayForClientActivity
{
    internal class PayForClientActivityHandler : IRequestHandler<PayForClientActivity, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayForClientActivityHandler(ISportActivityRepository SportActivityRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = SportActivityRepository;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(PayForClientActivity request, CancellationToken cancellationToken)
        {
            var acitivtyToPay = await _sportActivityRepository.GetSportActivityByIdAsync(request.ActivityId, cancellationToken);
            if (acitivtyToPay == null)
            {
                throw new SportActivityNotFoundException(request.ActivityId);
            }

            var paymentResult = await _employeeRepository.PayForActivityAsync(request.ActivityId, request.ClientEmail, cancellationToken);

            switch (paymentResult)
            {
                case PaymentResultEnum.Success:

                    return Unit.Value;

                case PaymentResultEnum.InsufficientFunds:

                    throw new PaymentFailedException();

                case PaymentResultEnum.ActivityNotFound:

                    throw new SportActivityNotFoundException(request.ActivityId);

                case PaymentResultEnum.ClientNotFound:

                    throw new ClientNotFoundException(request.ClientEmail);
                case PaymentResultEnum.AlreadyPaid:
                    throw new ActivityAlreadyPaidException(request.ActivityId);

                case PaymentResultEnum.ActivityCanceled:
                    throw new ActivityCanceledException(request.ActivityId);
            }

            return Unit.Value;
        }
    }
}
