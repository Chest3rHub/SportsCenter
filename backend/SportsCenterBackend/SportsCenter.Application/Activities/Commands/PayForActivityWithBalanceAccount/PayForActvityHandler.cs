using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;

namespace SportsCenter.Application.Activities.Commands.PayForActivity
{
    internal class PayForActvityHandler : IRequestHandler<PayForActivity, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayForActvityHandler(ISportActivityRepository SportActivityRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = SportActivityRepository;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(PayForActivity request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your account.");
            }

            var acitivtyToPay = await _sportActivityRepository.GetInstanceOfActivityAsync(request.InstanceOfActivityId, cancellationToken);
            if(acitivtyToPay == null)
            {
                throw new InstanceOfActivityNotFoundException(request.InstanceOfActivityId);
            }

            var paymentResult = await _employeeRepository.PayForActivityAsync(request.InstanceOfActivityId, userId, cancellationToken);

            switch (paymentResult)
            {
                case PaymentResultEnum.Success:

                    return Unit.Value;

                case PaymentResultEnum.InsufficientFunds:

                    throw new PaymentFailedException();

                case PaymentResultEnum.ActivityInstanceNotFound:

                    throw new InstanceOfActivityNotFoundException(request.InstanceOfActivityId);

                case PaymentResultEnum.ClientNotFound:

                    throw new ClientWithGivenIdNotFoundException(userId);

                case PaymentResultEnum.AlreadyPaid:
                    throw new ActivityAlreadyPaidException(request.InstanceOfActivityId); 

                case PaymentResultEnum.ActivityCanceled:
                    throw new ActivityCanceledException(request.InstanceOfActivityId);
            }

            return Unit.Value;
        }
    }
}
