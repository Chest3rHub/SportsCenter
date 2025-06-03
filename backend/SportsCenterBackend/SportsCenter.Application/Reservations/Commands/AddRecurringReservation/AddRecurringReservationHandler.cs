using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.EmployeesException;
using SportsCenter.Application.Exceptions.EmployeesExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Exceptions.SportActivitiesExceptions;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.TrainerAvailiabilityStatus;

namespace SportsCenter.Application.Reservations.Commands.AddRecurringReservation
{
    internal sealed class AddRecurringReservationHandler : IRequestHandler<AddRecurringReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICourtRepository _courtRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISportsCenterRepository _sportsCenterRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddRecurringReservationHandler(IReservationRepository reservationRepository, ICourtRepository courtRepository, IClientRepository clientRepository, IEmployeeRepository employeeRepository, ISportsCenterRepository sportsCenterRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _courtRepository = courtRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _sportsCenterRepository = sportsCenterRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddRecurringReservation request, CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetClientByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
            {
                throw new ClientWithGivenIdNotFoundException(request.ClientId);
            }

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

                var availabilityStatus = await _employeeRepository.IsTrainerAvailableAsync(request.TrainerId.Value, request.StartTime, request.StartTime.Hour * 60 + request.StartTime.Minute, request.EndTime.Hour * 60 + request.EndTime.Minute, cancellationToken);

                if (availabilityStatus == TrainerAvailabilityStatus.IsFired)
                {
                    throw new EmployeeAlreadyDismissedException(request.TrainerId.Value);
                }
            }

            if (request.ParticipantsCount > 8)
                throw new TooManyParticipantsException();

            DateTime currentDate = request.StartTime;
            List<FailedReservation> failedReservations = new List<FailedReservation>();
            List<ReservationProposal> reservationProposals = new List<ReservationProposal>();
            List<Rezerwacja> reservationsToAdd = new();

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

            while (currentDate <= request.RecurrenceEndDate)
            {
                DateTime endDate = currentDate.Add(request.EndTime - request.StartTime);

                var specialWorkingHours = await _sportsCenterRepository.GetSpecialWorkingHoursByDateAsync(currentDate.Date, cancellationToken);
                bool isValidHours = true;

                if (specialWorkingHours != null)
                {
                    WyjatkoweGodzinyPracy workingHours = specialWorkingHours;

                    int clubOpeningTimeInMinutes = workingHours.GodzinaOtwarcia.Hour * 60 + workingHours.GodzinaOtwarcia.Minute;
                    int clubClosingTimeInMinutes = workingHours.GodzinaZamkniecia.Hour * 60 + workingHours.GodzinaZamkniecia.Minute;

                    int reservationStartInMinutes = request.StartTime.Hour * 60 + request.StartTime.Minute;
                    int reservationEndInMinutes = request.EndTime.Hour * 60 + request.EndTime.Minute;

                    if (reservationStartInMinutes < clubOpeningTimeInMinutes || reservationEndInMinutes > clubClosingTimeInMinutes)
                    {
                        isValidHours = false;

                        failedReservations.Add(new FailedReservation
                        {
                            Date = currentDate,
                            Reason = $"Złe godziny pracy klubu. Klub w tym dniu ma godziny pracy od {workingHours.GodzinaOtwarcia:HH:mm} do {workingHours.GodzinaZamkniecia:HH:mm}."
                        });
                    }
                }
                else
                {
                    string dayOfWeek = dniTygodnia[currentDate.DayOfWeek];
                    var standardWorkingHours = await _sportsCenterRepository.GetWorkingHoursByDayAsync(dayOfWeek, cancellationToken);

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
                        isValidHours = false;

                        failedReservations.Add(new FailedReservation
                        {
                            Date = currentDate,
                            Reason = $"Złe godziny pracy klubu. Klub w tym dniu ma godziny pracy od {workingHours.GodzinaOtwarcia:HH:mm} do {workingHours.GodzinaZamkniecia:HH:mm}."
                        });
                    }
                }

                if (isValidHours)
                {

                    //czy w tym czasie klient jest zapisany na inne zaj lub ma zlozona rezerwacje
                    DateTime startDateTime = currentDate.Date
                        .AddHours(request.StartTime.Hour)
                        .AddMinutes(request.StartTime.Minute);

                    DateTime endDateTime = currentDate.Date
                        .AddHours(request.EndTime.Hour)
                        .AddMinutes(request.EndTime.Minute);

                    var isClientAvailable = await _reservationRepository.IsClientAvailableForPeriodAsync(
                        request.ClientId,
                        startDateTime,
                        endDateTime, 
                        cancellationToken);

                    if (!isClientAvailable)
                    {
                        failedReservations.Add(new FailedReservation
                        {
                            Date = currentDate,
                            Reason = $"Klient ma już inną rezerwację lub zajęcia."
                        });

                        currentDate = request.Recurrence switch
                        {
                            "Daily" => currentDate.AddDays(1),
                            "Weekly" => currentDate.AddDays(7),
                            "BiWeekly" => currentDate.AddDays(14),
                            "Monthly" => currentDate.AddMonths(1),
                            _ => throw new Exception("Invalid recurrence value")
                        };
                        continue;
                    }

                    bool isCourtAvailable = await _courtRepository.IsCourtAvailableAsync(request.CourtId, currentDate, endDate, cancellationToken);
                    bool isTrainerAvailable = true;

                    if (request.TrainerId.HasValue)
                    {
                        var trainerAvailability = await _employeeRepository.IsTrainerAvailableAsync(
                            request.TrainerId.Value,
                            currentDate,
                            currentDate.Hour * 60 + currentDate.Minute,
                            request.EndTime.Hour * 60 + request.EndTime.Minute,
                            cancellationToken);

                        if (trainerAvailability != TrainerAvailabilityStatus.Available)
                        {
                            isTrainerAvailable = false;
                        }
                    }

                    if (!isCourtAvailable || !isTrainerAvailable)
                    {
                        var availableCourts = isCourtAvailable ? null : await _courtRepository.GetAvailableCourtsAsync(currentDate, endDate, cancellationToken);
                        var availableTrainers = isTrainerAvailable ? null : await _employeeRepository.GetAvailableTrainersAsync(currentDate, endDate, cancellationToken);

                        reservationProposals.Add(new ReservationProposal
                        {
                            Date = currentDate,
                            AvailableCourts = availableCourts?.Select(c => c.KortId).ToList() ?? new List<int>(),
                            AvailableTrainers = availableTrainers?.Select(t => t.PracownikId).ToList() ?? new List<int>()
                        });

                    }
                    else
                    {
                        decimal cost = await CalculateCostAsync(request, currentDate, endDate, cancellationToken);
                        reservationsToAdd.Add(new Rezerwacja
                        {
                            KlientId = request.ClientId,
                            KortId = request.CourtId,
                            DataOd = currentDate,
                            DataDo = endDate,
                            DataStworzenia = DateOnly.FromDateTime(DateTime.UtcNow),
                            TrenerId = request.TrainerId,
                            CzyUwzglednicSprzet = request.IsEquipmentReserved,
                            Koszt = cost,
                            CzyOplacona = false,
                            CzyOdwolana = false,
                            CzyZwroconoPieniadze = false
                        });
                    }
                }

                currentDate = request.Recurrence switch
                {
                    "Daily" => currentDate.AddDays(1),
                    "Weekly" => currentDate.AddDays(7),
                    "BiWeekly" => currentDate.AddDays(14),
                    "Monthly" => currentDate.AddMonths(1),
                    _ => throw new Exception("Invalid recurrence value")
                };
            }

            // dodanie poprawnych rezerwacji
            foreach (var res in reservationsToAdd)
            {
                await _reservationRepository.AddReservationAsync(res, cancellationToken);
            }

            _httpContextAccessor.HttpContext!.Items["reservationResult"] = new
            {
                success = true,
                failedReservations,
                reservationProposals
            };

            return Unit.Value;
        }

        private async Task NotifyUserAboutFailedReservationsAsync(List<FailedReservation> failedReservations, List<ReservationProposal> reservationProposals)
        {
            foreach (var proposal in reservationProposals)
            {
                if (proposal.AvailableCourts.Any() || proposal.AvailableTrainers.Any())
                {
                    string availableCourts = string.Join(", ", proposal.AvailableCourts);
                    string availableTrainers = string.Join(", ", proposal.AvailableTrainers);

                    Console.WriteLine($"Propozycje dla daty {proposal.Date:yyyy-MM-dd}:");
                    Console.WriteLine($"Dostępne korty: {availableCourts}");
                    Console.WriteLine($"Dostępni trenerzy: {availableTrainers}");
                }
                else
                {
                    Console.WriteLine($"Brak dostępnych alternatyw dla daty {proposal.Date:yyyy-MM-dd}. Rezerwacja nie udała się.");
                }
            }

            foreach (var failedReservation in failedReservations)
            {

                Console.WriteLine(failedReservation.Reason);
            }

            await Task.CompletedTask;
        }

        public class ReservationProposal
        {
            public DateTime Date { get; set; }
            public List<int> AvailableCourts { get; set; } = new List<int>();
            public List<int> AvailableTrainers { get; set; } = new List<int>();
        }

        public class FailedReservation
        {
            public DateTime Date { get; set; }
            public string Reason { get; set; }
        }

        private async Task<decimal> CalculateCostAsync(AddRecurringReservation request, DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
            var reservationDurationInHours = (endTime - startTime).TotalHours;

            decimal cost = 0;

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

            return cost;
        }
    }
}