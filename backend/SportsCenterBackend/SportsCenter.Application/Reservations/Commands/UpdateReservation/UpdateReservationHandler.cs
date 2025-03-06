using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.UpdateReservation
{
    internal sealed class UpdateReservationHandler : IRequestHandler<UpdateReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateReservationHandler(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(UpdateReservation request, CancellationToken cancellationToken)
        {          
            var reservation = await _reservationRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);

            if (reservation == null)
            {
                throw new ReservationNotFoundException(request.ReservationId);
            }

            var isTrainerAvailable = await _reservationRepository.IsTrainerAvailableAsync(request.TrainerId, reservation.DataOd, reservation.DataDo, cancellationToken);
            if (!isTrainerAvailable)
            {
                throw new TrainerNotAvaliableException();
            }
        
            reservation.TrenerId = request.TrainerId;
 
            await _reservationRepository.UpdateReservationAsync(reservation, cancellationToken);

            return Unit.Value;
        }
    }
}
