using MediatR;
using Microsoft.AspNetCore.Http;
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

namespace SportsCenter.Application.Reservations.Commands.AddSingleReservationYourself
{
    internal sealed class AddSingleReservationYourselfHandler : IRequestHandler<AddSingleReservationYourself, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddSingleReservationYourselfHandler(IReservationRepository reservationRepository, IClientRepository clientRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _clientRepository = clientRepository;
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


            if (request.ParticipantsCount > 8)
                throw new TooManyParticipantsException();

            bool isCourtAvailable = await _reservationRepository.IsCourtAvailableAsync(request.CourtId, request.StartTime, request.EndTime, cancellationToken);
            if (!isCourtAvailable)
                throw new CourtNotAvaliableException(request.CourtId);

            if (request.TrainerId.HasValue)
            {
                bool isTrainerAvailable = await _reservationRepository.IsTrainerAvailableAsync(request.TrainerId.Value, request.StartTime, request.EndTime, cancellationToken);
                if (!isTrainerAvailable)
                    throw new TrainerNotAvaliableException();
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

            var discount = await _clientRepository.GetActivityDiscountForClientAsync(clientId, cancellationToken);
            if (discount.HasValue && discount.Value > 0)
            {
                cost *= (1 - discount.Value / 100m);
            }

            var newReservation = new Rezerwacja
            {
                KlientId = clientId,
                KortId = request.CourtId,
                DataOd = request.StartTime,
                DataDo = request.EndTime,
                DataStworzenia = DateOnly.FromDateTime(DateTime.UtcNow),
                TrenerId = request.TrainerId,
                CzyUwzglednicSprzet = request.IsEquipmentReserved,
                Koszt = cost
            };

            await _reservationRepository.AddReservationAsync(newReservation, cancellationToken);

            return Unit.Value;
        }
    }
}
