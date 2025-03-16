using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.CancelClientReservation
{
    internal class CancelClientReservationHandler : IRequestHandler<CancelClientReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CancelClientReservationHandler(IReservationRepository reservationRepository, IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(CancelClientReservation request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByEmailAsync(request.ClientEmail, cancellationToken);
            if (client == null)
            {
                throw new ClientNotFoundException(request.ClientEmail);
            }

            var reservationToCancel = await _reservationRepository.GetReservationByClientIdAsync(request.ReservationId, client.KlientId, cancellationToken);
            if (reservationToCancel == null)
            {
                throw new ReservationNotFoundException(request.ReservationId);
            }

            if (reservationToCancel.CzyOdwolana == true)
            {
                throw new ReservationAlreadyCanceledException(reservationToCancel.RezerwacjaId);
            }

            await _reservationRepository.CancelReservationAsync(request.ReservationId, cancellationToken);

            return Unit.Value;
        }
    }
}
