﻿using MediatR;
using Microsoft.AspNetCore.Http;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.ReservationExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//jak sprawdzac czy kort i trener sa wolni
//jak dodawac koszt rezerwacji? musi byc liczony na jakiejs podstawie 

namespace SportsCenter.Application.Reservations.Commands.AddReservation
{
    internal sealed class AddReservationHandler : IRequestHandler<AddReservation, Unit>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddReservationHandler(IReservationRepository reservationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reservationRepository = reservationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddReservation request, CancellationToken cancellationToken)
        {

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

            var newReservation = new Rezerwacja
            {
                KlientId = request.ClientId,
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
