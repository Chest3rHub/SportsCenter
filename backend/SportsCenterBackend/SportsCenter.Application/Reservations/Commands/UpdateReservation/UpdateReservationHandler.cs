using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

namespace SportsCenter.Application.Reservations.Commands.UpdateReservation
{
    internal sealed class UpdateReservationHandler : IRequestHandler<UpdateReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateReservationHandler(IReservationRepository reservationRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(UpdateReservation request, CancellationToken cancellationToken)
        {          
            var reservation = await _reservationRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);

            if (reservation == null)
            {
                throw new ReservationNotFoundException(request.ReservationId);
            }

            var trainerPosition = await _employeeRepository.GetEmployeePositionNameByIdAsync(request.TrainerId, cancellationToken);
            if (trainerPosition == null)
            {
                throw new EmployeeNotFoundException(request.TrainerId);
            }

            if (trainerPosition != "Trener")//musi byc Trener w bazie w tabeli TypPracownika
            {
                throw new NotTrainerEmployeeException(request.TrainerId);
            }

            var (startDate, endDate) = await _reservationRepository.GetReservationDetailsByIdAsync(request.ReservationId);

            TimeOnly startHourTimeOnly = TimeOnly.FromDateTime(startDate);
            TimeOnly endHourTimeOnly = TimeOnly.FromDateTime(endDate);

            var availabilityStatus = await _employeeRepository.IsTrainerAvailableAsync(request.TrainerId, startDate, startHourTimeOnly.Hour * 60 + startHourTimeOnly.Minute, endHourTimeOnly.Hour * 60 + endHourTimeOnly.Minute, cancellationToken);

            if (availabilityStatus == TrainerAvailabilityStatus.IsFired)
            {
                throw new EmployeeAlreadyDismissedException(request.TrainerId);
            }

            if (availabilityStatus != TrainerAvailabilityStatus.Available)
            {
                throw new TrainerNotAvaliableException();
            }

            reservation.TrenerId = request.TrainerId;
 
            await _reservationRepository.UpdateReservationAsync(reservation, cancellationToken);

            return Unit.Value;
        }
    }
}
