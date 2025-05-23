using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    internal class ReservationRepository : IReservationRepository
    {
        private SportsCenterDbContext _dbContext;

        public ReservationRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken)
        {
            await _dbContext.Rezerwacjas.AddAsync(reservation, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Rezerwacja> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken)
        {
            return await _dbContext.Rezerwacjas
                .FirstOrDefaultAsync(r => r.RezerwacjaId == reservationId, cancellationToken);
        }

        public async Task UpdateReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken)
        {
            _dbContext.Rezerwacjas.Update(reservation);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken)
        {
            _dbContext.Rezerwacjas.Remove(reservation);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<IEnumerable<Rezerwacja>> GetReservationsByTrainerIdAsync(int trainerId, CancellationToken cancellationToken)
        {
            return await _dbContext.Rezerwacjas
                .Where(r => r.TrenerId == trainerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsTrainerAssignedToReservationAsync(int reservationId, int trainerId)
        {
            return await _dbContext.Rezerwacjas
                .Where(r => r.RezerwacjaId == reservationId && r.TrenerId == trainerId &&
                    r.CzyOdwolana == false)
                .AnyAsync();
        }

        public async Task<(DateTime DataOd, DateTime DataDo)> GetReservationDetailsByIdAsync(int reservationId)
        {          
            var reservation = await _dbContext.Rezerwacjas
                .Where(r => r.RezerwacjaId == reservationId)
                .Select(r => new { r.DataOd, r.DataDo })
                .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return (DateTime.MinValue, DateTime.MinValue);
            }
            return (reservation.DataOd, reservation.DataDo);
        }
        public async Task<(DateTime startDateTime, DateTime endDateTime)?> GetReservationDetailsAsync(int reservationId, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Rezerwacjas
                .Where(r => r.RezerwacjaId == reservationId)
                .Select(r => new { r.DataOd, r.DataDo })
                .FirstOrDefaultAsync(cancellationToken);

            if (reservation == null)
                return null;

            return (reservation.DataOd, reservation.DataDo);
        }
        public async Task<bool> HasClientReservation(int reservationId, int clientId, CancellationToken cancellationToken)
        {
            return await _dbContext.Rezerwacjas
                .AnyAsync(r => r.RezerwacjaId == reservationId && r.KlientId == clientId  && r.CzyOdwolana == false ,cancellationToken);
        }
        public async Task CancelReservationAsync(int reservationId, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Rezerwacjas
                .FirstOrDefaultAsync(r => r.RezerwacjaId == reservationId, cancellationToken);        

            if (reservation.CzyOplacona == true)
            {
                var client = await _dbContext.Klients
                    .FirstOrDefaultAsync(c => c.KlientId == reservation.KlientId, cancellationToken);

                client.Saldo += reservation.Koszt;
                reservation.CzyZwroconoPieniadze = true;
            }
            reservation.CzyOdwolana = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Rezerwacja> GetReservationByClientIdAsync(int reservationId, int clientId, CancellationToken cancellationToken)
        {
            return await _dbContext.Rezerwacjas
                .FirstOrDefaultAsync(r => r.RezerwacjaId == reservationId && r.KlientId == clientId, cancellationToken);
        }
        public async Task<bool> IsClientAvailableForPeriodAsync(int clientId, DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken)
        {
            //Sprawdzenie kolizji z zaj klienta tymi ktore NIE są odwołane
            var conflictList = _dbContext.InstancjaZajecKlients
                .Where(ik => ik.KlientId == clientId && ik.DataWypisu == null) // tylko aktywne zapisy
                .Join(_dbContext.InstancjaZajecs,
                    ik => ik.InstancjaZajecId,
                    iz => iz.InstancjaZajecId,
                    (ik, iz) => iz)
                .Where(iz => !iz.CzyOdwolane.HasValue || iz.CzyOdwolane == false)
                .Join(_dbContext.GrafikZajecs,
                    iz => iz.GrafikZajecId,
                    g => g.GrafikZajecId,
                    (iz, g) => new { iz.Data, g.GodzinaOd, g.CzasTrwania })
                .ToList();

            var hasActivityConflict = conflictList.Any(x =>
            {
                var start = x.Data.ToDateTime(TimeOnly.FromTimeSpan(x.GodzinaOd));
                var end = start.AddMinutes(x.CzasTrwania);
                return start < endDateTime && end > startDateTime;
            });

            if (hasActivityConflict)
                return false;

            //Sprawdzenie kolizji z aktywnymi rezerwacjami
            var hasReservationConflict = await _dbContext.Rezerwacjas
                .AnyAsync(r =>
                    r.KlientId == clientId &&
                    r.DataOd < endDateTime &&
                    r.DataDo > startDateTime &&
                    r.CzyOdwolana == false,
                    cancellationToken);

            return !hasReservationConflict;
        }

    }
}
