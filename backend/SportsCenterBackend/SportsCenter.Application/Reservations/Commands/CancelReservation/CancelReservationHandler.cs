using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.CancelReservation
{
    internal class CancelReservationHandler : IRequestHandler<CancelReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CancelReservationHandler(IReservationRepository reservationRepository, IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(CancelReservation request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your account.");
            }

            var reservationToCancel = await _reservationRepository.GetReservationByClientIdAsync(request.ReservationId, userId, cancellationToken);
            if (reservationToCancel == null)
            {
                throw new ReservationNotFoundException(request.ReservationId);
            }

            if (reservationToCancel.CzyOdwolana == true)
            {
                throw new ReservationAlreadyCanceledException(reservationToCancel.RezerwacjaId);
            }

            if (reservationToCancel.DataOd < DateTime.Now.AddHours(2))
            {
                throw new TooLateToCancelreservationException(request.ReservationId);
            }

            await _reservationRepository.CancelReservationAsync(request.ReservationId, cancellationToken);
            
            return Unit.Value;
        }
    }
}
