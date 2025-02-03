using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Employees.Commands.RemoveTask;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.RemoveReservation
{
    internal sealed class RemoveReservationHandler : IRequestHandler<RemoveReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveReservationHandler(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(RemoveReservation request, CancellationToken cancellationToken)
        {         
            var reservation = await _reservationRepository.GetReservationByIdAsync(request.Id, cancellationToken);

            if (reservation == null)
            {
                throw new ReservationNotFoundException(request.Id);
            }
           
            var remainingTime = reservation.DataOd - DateTime.UtcNow;
           
            if (remainingTime.TotalHours < 24)
            {
                throw new InvalidOperationException("Reservation can only be canceled if the remaining time is greater than or equal to 24 hours.");
            }         

            await _reservationRepository.DeleteReservationAsync(reservation, cancellationToken);

            return Unit.Value;
        }
    }
}
