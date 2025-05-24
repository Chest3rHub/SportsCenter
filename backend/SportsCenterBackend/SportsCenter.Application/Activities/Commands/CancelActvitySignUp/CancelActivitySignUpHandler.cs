using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Enums;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.ClientEntryStatus;

namespace SportsCenter.Application.Activities.Commands.CancelActvitySignUp
{
    internal class CancelActivitySignUpHandler : IRequestHandler<CancelActivitySignUp, Unit>
    {
        private readonly ISportActivityRepository _sportActivityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CancelActivitySignUpHandler(ISportActivityRepository SportActivityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _sportActivityRepository = SportActivityRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(CancelActivitySignUp request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int clientId))
            {
                throw new UnauthorizedAccessException("User must be logged in.");
            }

            var signUp = await _sportActivityRepository.GetInstanceClientEntryAsync(request.InstanceOfActivityId, clientId, cancellationToken);

            switch (signUp)
            {
                case EntryStatus.AlreadyUnsubscribed:
                    throw new ClientIsNotSignedUpException(clientId);

                case EntryStatus.PastEvent:
                    throw new CannotUnsubscribeFromPastEventException();

                case EntryStatus.Success:
                    return Unit.Value;

                default:
                    throw new InvalidOperationException($"Unhandled entry status: {signUp}");
            }
        }
    }
}
