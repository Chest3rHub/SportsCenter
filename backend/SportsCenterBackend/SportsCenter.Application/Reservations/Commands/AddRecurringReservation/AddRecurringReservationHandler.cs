using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Application.Reservations.Commands.AddReservation;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.AddRecurringReservation
{
    internal sealed class AddRecurringReservationHandler : IRequestHandler<AddRecurringReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddRecurringReservationHandler(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddRecurringReservation request, CancellationToken cancellationToken)
        {
            DateTime currentDate = request.StartTime;
            List<DateTime> failedReservations = new List<DateTime>();
            List<ReservationProposal> reservationProposals = new List<ReservationProposal>();

            while (currentDate <= request.RecurrenceEndDate)
            {
                DateTime endDate = currentDate.Add(request.EndTime - request.StartTime);
                bool isCourtAvailable = await _reservationRepository.IsCourtAvailableAsync(request.CourtId, currentDate, endDate, cancellationToken);
                bool isTrainerAvailable = request.TrainerId.HasValue
                    ? await _reservationRepository.IsTrainerAvailableAsync(request.TrainerId.Value, currentDate, endDate, cancellationToken)
                    : true;

                if (!isCourtAvailable || !isTrainerAvailable)
                {
                    
                    failedReservations.Add(currentDate);

                    if (!isCourtAvailable || !isTrainerAvailable)
                    {
                        var availableCourts = isCourtAvailable ? null : await _reservationRepository.GetAvailableCourtsAsync(currentDate, endDate, cancellationToken);
                        var availableTrainers = isTrainerAvailable ? null : await _reservationRepository.GetAvailableTrainersAsync(currentDate, endDate, cancellationToken);

                        reservationProposals.Add(new ReservationProposal
                        {
                            Date = currentDate,
                            AvailableCourts = availableCourts?.Select(c => c.KortId).ToList() ?? new List<int>(),
                            AvailableTrainers = availableTrainers?.Select(t => t.PracownikId).ToList() ?? new List<int>()
                        });

                        Console.WriteLine($"Nie udało się utworzyć rezerwacji na {currentDate:yyyy-MM-dd}. " +
                            $"Dostępne korty: {string.Join(", ", availableCourts?.Select(c => c.KortId.ToString()) ?? new List<string>())}. " +
                            $"Dostępni trenerzy: {string.Join(", ", availableTrainers?.Select(t => t.PracownikId.ToString()) ?? new List<string>())}.");
                    }
                }
                else
                {                 
                    decimal cost = CalculateCost(request, currentDate, endDate);

                    var newReservation = new Rezerwacja
                    {
                        KlientId = request.ClientId,
                        KortId = request.CourtId,
                        DataOd = currentDate,
                        DataDo = endDate,
                        DataStworzenia = DateOnly.FromDateTime(DateTime.UtcNow),
                        TrenerId = request.TrainerId,
                        CzyUwzglednicSprzet = request.IsEquipmentReserved,
                        Koszt = cost
                    };

                    await _reservationRepository.AddReservationAsync(newReservation, cancellationToken);
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

            if (failedReservations.Any())
            {
                await NotifyUserAboutFailedReservationsAsync(failedReservations, reservationProposals);
            }

            return Unit.Value;
        }


        private async Task NotifyUserAboutFailedReservationsAsync(List<DateTime> failedReservations, List<ReservationProposal> reservationProposals)
        {
            foreach (var failedDate in failedReservations)
            {
                var proposals = reservationProposals.Where(p => p.Date == failedDate).FirstOrDefault();
                if (proposals != null)
                {
                    string availableCourts = string.Join(", ", proposals.AvailableCourts);
                    string availableTrainers = string.Join(", ", proposals.AvailableTrainers);

                    Console.WriteLine($"Propozycje dla daty {failedDate}:");
                    Console.WriteLine($"Dostępne korty: {availableCourts}");
                    Console.WriteLine($"Dostępni trenerzy: {availableTrainers}");
                }
                else
                {
                    Console.WriteLine($"Brak dostępnych alternatyw dla daty {failedDate}. Brak wolnych kortów lub trenerów.");
                }
            }

            await Task.CompletedTask;
        }

        public class ReservationProposal
        {
            public DateTime Date { get; set; }
            public List<int> AvailableCourts { get; set; } = new List<int>();
            public List<int> AvailableTrainers { get; set; } = new List<int>();
        }



        private decimal CalculateCost(AddRecurringReservation request, DateTime startTime, DateTime endTime)
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

            return cost;
        }
    }
}
