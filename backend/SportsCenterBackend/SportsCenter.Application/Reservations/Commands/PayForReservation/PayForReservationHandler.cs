using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.PaymentResult;

namespace SportsCenter.Application.Reservations.Commands.PayForReservation
{
    internal class PayForReservationHandler : IRequestHandler<PayForReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayForReservationHandler(IReservationRepository reservationRepository, IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(PayForReservation request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your account.");
            }

            var reservationToPay = await _reservationRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);
            if (reservationToPay == null)
            {
                throw new ReservationNotFoundException(request.ReservationId);
            }

            var paymentResult = await _clientRepository.PayForReservationAsync(request.ReservationId, userId, cancellationToken);

            switch (paymentResult)
            {
                case PaymentResultEnum.Success:

                    return Unit.Value;

                case PaymentResultEnum.InsufficientFunds:

                    throw new PaymentFailedException();

                case PaymentResultEnum.ActivityInstanceNotFound:

                    throw new ReservationNotFoundException(request.ReservationId);

                case PaymentResultEnum.ClientNotFound:

                    throw new ClientWithGivenIdNotFoundException(userId);

                case PaymentResultEnum.AlreadyPaid:

                    throw new ReservationAlreadyPaidException(request.ReservationId);
            }

            return Unit.Value;
        }
    }
}
