using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.CourtsExceptions;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

namespace SportsCenter.Application.Reservations.Commands.AddSingleReservationYourself
{
    internal sealed class AddSingleReservationYourselfHandler : IRequestHandler<AddSingleReservationYourself, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICourtRepository _courtRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISportsCenterRepository _sportsCenterRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddSingleReservationYourselfHandler(IReservationRepository reservationRepository, ICourtRepository courtRepository, IClientRepository clientRepository, ISportsCenterRepository sportsCenterRepository, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _courtRepository = courtRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _sportsCenterRepository = sportsCenterRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddSingleReservationYourself request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                throw new UnauthorizedAccessException("No user authorization.");
            }

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!int.TryParse(userIdClaim, out int clientId))
            {
                throw new UnauthorizedAccessException("Invalid user ID.");
            }

            if (!user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Klient"))
            {
                throw new UnauthorizedAccessException("Only customers can make reservations.");
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

            var startTime = DateTime.Parse(request.StartTime);
            var endTime = DateTime.Parse(request.EndTime);

            Console.WriteLine("AAAAAAAAAAAAAAAAAA request start time: " + startTime);
            Console.WriteLine("AAAAAAAAAAAAAAAAAA request end time: " + endTime);

            var specialWorkingHours = await _sportsCenterRepository.GetSpecialWorkingHoursByDateAsync(startTime.Date, cancellationToken);

            if (specialWorkingHours != null)
            {
                WyjatkoweGodzinyPracy workingHours = specialWorkingHours;

                int clubOpeningTimeInMinutes = workingHours.GodzinaOtwarcia.Hour * 60 + workingHours.GodzinaOtwarcia.Minute;
                int clubClosingTimeInMinutes = workingHours.GodzinaZamkniecia.Hour * 60 + workingHours.GodzinaZamkniecia.Minute;

                Console.WriteLine("AAAAAAAAAAAAAAAAAA club open time in min: " + clubOpeningTimeInMinutes);
                Console.WriteLine("AAAAAAAAAAAAAAAAAA club close time in min: " + clubClosingTimeInMinutes);

                int reservationStartInMinutes = startTime.Hour * 60 + startTime.Minute;
                int reservationEndInMinutes = endTime.Hour * 60 + endTime.Minute;

                Console.WriteLine("AAAAAAAAAAAAAAAAAA reservation start time in min: " + reservationStartInMinutes);
                Console.WriteLine("AAAAAAAAAAAAAAAAAA reservation end time in min: " + reservationEndInMinutes);

                if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
                {
                    throw new ReservationOutsideWorkingHoursException();
                }
            }
            else
            {
                string dayOfWeek = dniTygodnia[startTime.DayOfWeek];
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

                Console.WriteLine("AAAAAAAAAAAAAAAAAA club open time in min: " + clubOpeningTimeInMinutes);
                Console.WriteLine("AAAAAAAAAAAAAAAAAA club close time in min: " + clubClosingTimeInMinutes);

                int reservationStartInMinutes = startTime.Hour * 60 + startTime.Minute;
                int reservationEndInMinutes = endTime.Hour * 60 + endTime.Minute;

                Console.WriteLine("AAAAAAAAAAAAAAAAAA reservation start time in min: " + reservationStartInMinutes);
                Console.WriteLine("AAAAAAAAAAAAAAAAAA reservation end time in min: " + reservationEndInMinutes);

                if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
                {
                    throw new ReservationOutsideWorkingHoursException();
                }
            }

            if (request.ParticipantsCount > 8)
                throw new TooManyParticipantsException();

            bool isCourtAvailable = await _courtRepository.IsCourtAvailableAsync(request.CourtId, startTime, endTime, cancellationToken);
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

                if (trainerPosition != "Trener")
                {
                    throw new NotTrainerEmployeeException(request.TrainerId.Value);
                }

                var availabilityStatus = await _employeeRepository.IsTrainerAvailableAsync(request.TrainerId.Value, startTime, startTime.Hour * 60 + startTime.Minute, endTime.Hour * 60 + endTime.Minute, cancellationToken);

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

            var reservationDurationInHours = (endTime - startTime).TotalHours;

            cost += (decimal)(reservationDurationInHours * 70);

            if (request.TrainerId.HasValue)
            {
                cost += (decimal)(reservationDurationInHours * 50);
            }

            if (request.IsEquipmentReserved)
            {
                cost += 10;
            }

            var discount = await _clientRepository.GetActivityDiscountForClientAsync(clientId, cancellationToken);
            if (discount.HasValue && discount.Value > 0)
            {
                cost *= (1 - discount.Value / 100m);
            }

            var newReservation = new Rezerwacja
            {
                KlientId = clientId,
                KortId = request.CourtId,
                DataOd = startTime,
                DataDo = endTime,
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
