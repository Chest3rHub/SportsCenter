using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.CourtsExceptions;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SportsCenter.Application.Reservations.Commands.AddReservation
{
    internal sealed class AddSingleReservationHandler : IRequestHandler<AddSingleReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICourtRepository _courtRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ISportsCenterRepository _sportsCenterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddSingleReservationHandler(IReservationRepository reservationRepository, ICourtRepository courtRepository, IClientRepository clientRepository, IEmployeeRepository employeeRepository, ISportsCenterRepository sportsCenterRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _courtRepository = courtRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _sportsCenterRepository = sportsCenterRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddSingleReservation request, CancellationToken cancellationToken)
        {

            var client = await _clientRepository.GetClientByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
            {
                throw new ClientWithGivenIdNotFoundException(request.ClientId);
            }

            var daysOfWeek = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "poniedzialek" },
                { DayOfWeek.Tuesday, "wtorek" },
                { DayOfWeek.Wednesday, "sroda" },
                { DayOfWeek.Thursday, "czwartek" },
                { DayOfWeek.Friday, "piatek" },
                { DayOfWeek.Saturday, "sobota" },
                { DayOfWeek.Sunday, "niedziela" }
             };


            //czy w tym czasie klient jest zapisany na inne zaj lub ma zlozona rezerwacje
            var isAvailable = await _reservationRepository.IsClientAvailableForPeriodAsync(request.ClientId, request.StartTime, request.EndTime, cancellationToken);

            if (!isAvailable)
            {
                throw new ClientAlreadyHasActivityOrReservationException();
            }


            var specialWorkingHours = await _sportsCenterRepository.GetSpecialWorkingHoursByDateAsync(request.StartTime.Date, cancellationToken);

            if (specialWorkingHours != null)
            {
                WyjatkoweGodzinyPracy workingHours = specialWorkingHours;

                int clubOpeningTimeInMinutes = workingHours.GodzinaOtwarcia.Hour * 60 + workingHours.GodzinaOtwarcia.Minute;
                int clubClosingTimeInMinutes = workingHours.GodzinaZamkniecia.Hour * 60 + workingHours.GodzinaZamkniecia.Minute;

                int reservationStartInMinutes = request.StartTime.Hour * 60 + request.StartTime.Minute;
                int reservationEndInMinutes = request.EndTime.Hour * 60 + request.EndTime.Minute;

                if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
                {
                    throw new ReservationOutsideWorkingHoursException();
                }
            }
            else
            {
                string dayOfWeek = daysOfWeek[request.StartTime.DayOfWeek];
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

                int reservationStartInMinutes = request.StartTime.Hour * 60 + request.StartTime.Minute;
                int reservationEndInMinutes = request.EndTime.Hour * 60 + request.EndTime.Minute;

                if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
                {
                    throw new ReservationOutsideWorkingHoursException();
                }
            }

            if (request.ParticipantsCount > 8)
                throw new TooManyParticipantsException();

            bool isCourtAvailable = await _courtRepository.IsCourtAvailableAsync(request.CourtId, request.StartTime, request.EndTime, cancellationToken);
            if (!isCourtAvailable)
                throw new CourtNotAvaliableException(request.CourtId);

            if (request.TrainerId.HasValue && request.TrainerId.Value == 0)
            {
                request.TrainerId = null;
            }

            if (request.TrainerId.HasValue)
            {
                var trainerPosition = await _employeeRepository.GetEmployeePositionNameByIdAsync(request.TrainerId.Value, cancellationToken);
                if (trainerPosition == null)
                {
                    throw new EmployeeNotFoundException(request.TrainerId.Value);
                }

                if (trainerPosition != "Trener")//musi byc Trener w bazie w tabeli TypPracownika
                {
                    throw new NotTrainerEmployeeException(request.TrainerId.Value);
                }

                var availabilityStatus = await _employeeRepository.IsTrainerAvailableAsync(request.TrainerId.Value,request.StartTime,request.StartTime.Hour * 60 + request.StartTime.Minute,request.EndTime.Hour * 60 + request.EndTime.Minute,cancellationToken);

                if (availabilityStatus == TrainerAvailabilityStatus.IsFired)
                {
                    throw new EmployeeAlreadyDismissedException(request.TrainerId.Value);
                }

                if (availabilityStatus != TrainerAvailabilityStatus.Available)
                {
                    throw new TrainerNotAvaliableException();
                }
            }

            //proponowana logika liczenia kosztu
            //za 1h rezerwacji kortu (samego) 70 zl
            //za kazda h rezerwacji trenera 50 zl
            //za sprzet jednorazowo w ramach rezerwacji 10 zl (nie co godzine)
            decimal cost = 0;

            var reservationDurationInHours = (request.EndTime - request.StartTime).TotalHours;

            cost += (decimal)(reservationDurationInHours * 70);

            if (request.TrainerId.HasValue)
            {
                cost += (decimal)(reservationDurationInHours * 50);
            }

            if (request.IsEquipmentReserved)
            {
                cost += 10;
            }

            var discount = await _clientRepository.GetActivityDiscountForClientAsync(request.ClientId, cancellationToken);
            if (discount.HasValue && discount.Value > 0)
            {
                cost *= (1 - discount.Value / 100m);
            }

            var newReservation = new Rezerwacja
            {
                KlientId = request.ClientId,
                KortId = request.CourtId,
                DataOd = request.StartTime,
                DataDo = request.EndTime,
                DataStworzenia = DateOnly.FromDateTime(DateTime.UtcNow),
                TrenerId = request.TrainerId,
                CzyUwzglednicSprzet = request.IsEquipmentReserved,
                Koszt = cost,
                CzyOplacona = false,
                CzyOdwolana = false,
                CzyZwroconoPieniadze = false
            };

            await _reservationRepository.AddReservationAsync(newReservation, cancellationToken);

            return Unit.Value;
        }
    }
}
