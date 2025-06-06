﻿using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface IReservationRepository
    {
        Task AddReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken);
        Task<Rezerwacja> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken);
        Task UpdateReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken);
        Task DeleteReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken);       
        Task<IEnumerable<Rezerwacja>> GetReservationsByTrainerIdAsync(int trainerId, CancellationToken cancellationToken);
        Task<bool> IsTrainerAssignedToReservationAsync(int reservationId, int trainerId);
        Task<(DateTime DataOd, DateTime DataDo)> GetReservationDetailsByIdAsync(int reservationId);
        Task<(DateTime startDateTime, DateTime endDateTime)?> GetReservationDetailsAsync(int reservationId, CancellationToken cancellationToken);
        Task<bool> HasClientReservation(int reservationId, int clientId, CancellationToken cancellationToken);
        Task CancelReservationAsync(int reservationId, CancellationToken cancellationToken);
        Task<Rezerwacja> GetReservationByClientIdAsync(int reservationId, int clientId, CancellationToken cancellationToken);
        Task<bool> IsClientAvailableForPeriodAsync(int clientId, DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken);
    }
}
