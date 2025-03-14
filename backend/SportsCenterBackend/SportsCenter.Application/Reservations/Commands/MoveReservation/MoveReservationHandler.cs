using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.CourtsExceptions;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

namespace SportsCenter.Application.Reservations.Commands.MoveReservation
{
    internal class MoveReservationHandler : IRequestHandler<MoveReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISportsCenterRepository _sportsCenterRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MoveReservationHandler(IReservationRepository reservationRepository, ISportsCenterRepository sportsCenterRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _sportsCenterRepository = sportsCenterRepository;
            _employeeRepository = employeeRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Unit> Handle(MoveReservation request, CancellationToken cancellationToken)
        {          
            var reservation = await _reservationRepository.GetReservationByIdAsync(request.ReservationId, cancellationToken);

            if (reservation == null)
                throw new ReservationNotFoundException(request.ReservationId);

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("No permission to move reservation.");
            }

            var hasClientReservation = await _reservationRepository.HasClientReservation(request.ReservationId, userId, cancellationToken);

            if (!hasClientReservation)
            {
                throw new NotThatClientReservationException(userId, request.ReservationId);
            }

            var dniTygodnia = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "poniedzialek" },
                { DayOfWeek.Tuesday, "wtorek" },
                { DayOfWeek.Wednesday, "sroda" },
                { DayOfWeek.Thursday, "czwartek" },
                { DayOfWeek.Friday, "piatek" },
                { DayOfWeek.Saturday, "sobota" },
                { DayOfWeek.Sunday, "niedziela" }
             };

            var specialWorkingHours = await _sportsCenterRepository.GetSpecialWorkingHoursByDateAsync(request.NewStartTime.Date, cancellationToken);

            if (specialWorkingHours != null)
            {
                WyjatkoweGodzinyPracy workingHours = specialWorkingHours;

                int clubOpeningTimeInMinutes = workingHours.GodzinaOtwarcia.Hour * 60 + workingHours.GodzinaOtwarcia.Minute;
                int clubClosingTimeInMinutes = workingHours.GodzinaZamkniecia.Hour * 60 + workingHours.GodzinaZamkniecia.Minute;

                int reservationStartInMinutes = request.NewStartTime.Hour * 60 + request.NewStartTime.Minute;
                int reservationEndInMinutes = request.NewEndTime.Hour * 60 + request.NewEndTime.Minute;

                if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
                {
                    throw new ReservationOutsideWorkingHoursException();
                }
            }
            else
            {
                string dayOfWeek = dniTygodnia[request.NewStartTime.DayOfWeek];
                var standardWorkingHours = await _sportsCenterRepository.GetWorkingHoursByDayAsync(dayOfWeek, cancellationToken);

                if (standardWorkingHours == null)
                {
                    throw new ReservationOutsideWorkingHoursException();
                }

                GodzinyPracyKlubu workingHours = new GodzinyPracyKlubu
                {
                    GodzinaOtwarcia = standardWorkingHours.GodzinaOtwarcia,
                    GodzinaZamkniecia = standardWorkingHours.GodzinaZamkniecia
                };

                int clubOpeningTimeInMinutes = workingHours.GodzinaOtwarcia.Hour * 60 + workingHours.GodzinaOtwarcia.Minute;
                int clubClosingTimeInMinutes = workingHours.GodzinaZamkniecia.Hour * 60 + workingHours.GodzinaZamkniecia.Minute;

                int reservationStartInMinutes = request.NewStartTime.Hour * 60 + request.NewStartTime.Minute;
                int reservationEndInMinutes = request.NewEndTime.Hour * 60 + request.NewEndTime.Minute;

                if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
                {
                    throw new ReservationOutsideWorkingHoursException();
                }
            }

            if ((reservation.DataOd - DateTime.UtcNow).TotalHours < 24)
            {
              throw new PostponeReservationNotAllowedException();
            }

            bool isCourtAvailable = await _reservationRepository.IsCourtAvailableAsync(reservation.KortId, request.NewStartTime, request.NewEndTime, cancellationToken);
            if (!isCourtAvailable)
                throw new CourtNotAvaliableException(reservation.KortId);

            if (reservation.TrenerId.HasValue)
            {
                var trainerPosition = await _employeeRepository.GetEmployeePositionNameByIdAsync(reservation.TrenerId.Value, cancellationToken);
                if (trainerPosition == null)
                {
                    throw new EmployeeNotFoundException(reservation.TrenerId.Value);
                }

                if (trainerPosition != "Trener")//musi byc Trener w bazie w tabeli TypPracownika
                {
                    throw new NotTrainerEmployeeException(reservation.TrenerId.Value);
                }

                var availabilityStatus = await _employeeRepository.IsTrainerAvailableAsync(reservation.TrenerId.Value, request.NewStartTime, request.NewStartTime.Hour * 60 + request.NewStartTime.Minute, request.NewEndTime.Hour * 60 + request.NewEndTime.Minute, cancellationToken);

                if (availabilityStatus == TrainerAvailabilityStatus.IsFired)
                {
                    throw new EmployeeAlreadyDismissedException(reservation.TrenerId.Value);
                }

                if (availabilityStatus != TrainerAvailabilityStatus.Available)
                {
                    throw new TrainerNotAvaliableException();
                }
            }
           
            reservation.DataOd = request.NewStartTime;
            reservation.DataDo = request.NewEndTime;
           
            await _reservationRepository.UpdateReservationAsync(reservation, cancellationToken);

            return Unit.Value;
        }
    }

}