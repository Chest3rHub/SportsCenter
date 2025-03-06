using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.MoveReservation
{
    internal class MoveReservationHandler : IRequestHandler<MoveReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoveReservationHandler(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Unit> Handle(MoveReservation request, CancellationToken cancellationToken)
        {          
            var reservation = await _reservationRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);

            if (reservation == null)
                throw new ReservationNotFoundException(request.ReservationId);
       
                if ((reservation.DataOd - DateTime.UtcNow).TotalHours < 24)
                {
                    throw new InvalidOperationException("Client can only postpone the reservation up to 24 hours before its start date.");
                }

            bool isCourtAvailable = await _reservationRepository.IsCourtAvailableAsync(reservation.KortId, request.NewStartTime, request.NewEndTime, cancellationToken);
            if (!isCourtAvailable)
                throw new CourtNotAvaliableException(reservation.KortId);

            if (reservation.TrenerId.HasValue)
            {
                bool isTrainerAvailable = await _reservationRepository.IsTrainerAvailableAsync(reservation.TrenerId.Value, request.NewStartTime, request.NewEndTime, cancellationToken);
                if (!isTrainerAvailable)
                    throw new TrainerNotAvaliableException();
            }
           
            reservation.DataOd = request.NewStartTime;
            reservation.DataDo = request.NewEndTime;
           
            await _reservationRepository.UpdateReservationAsync(reservation, cancellationToken);

            return Unit.Value;
        }
    }

}