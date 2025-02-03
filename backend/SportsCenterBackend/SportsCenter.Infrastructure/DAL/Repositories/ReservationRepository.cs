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

        public async Task<bool> IsCourtAvailableAsync(int courtId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {           
            bool isReserved = await _dbContext.Rezerwacjas
                .AnyAsync(r => r.KortId == courtId &&
                               r.DataOd < endTime &&
                               r.DataDo > startTime, cancellationToken);
            
            var overlappingReservations = await _dbContext.GrafikZajecs
                .Where(gz => gz.KortId == courtId)
                .Join(_dbContext.DataZajecs,
                    gz => gz.GrafikZajecId,
                    dz => dz.GrafikZajecId,
                    (gz, dz) => new { gz, dz })
                .Where(x =>
                    (x.dz.Date >= startTime && x.dz.Date < endTime)
                    || (x.dz.Date.AddMinutes(x.gz.CzasTrwania) > startTime && x.dz.Date < endTime)
                )
                .AnyAsync(cancellationToken);

            return !isReserved && !overlappingReservations;
        }

        public async Task<bool> IsTrainerAvailableAsync(int trainerId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
            bool isReserved = await _dbContext.Rezerwacjas
                .AnyAsync(r => r.TrenerId == trainerId &&
                               r.DataOd < endTime &&
                               r.DataDo > startTime, cancellationToken);

            var overlappingReservations = await _dbContext.GrafikZajecs
                .Where(gz => gz.PracownikId == trainerId)
                .Join(_dbContext.DataZajecs,
                    gz => gz.GrafikZajecId,
                    dz => dz.GrafikZajecId,
                    (gz, dz) => new { gz, dz })
                .Where(x =>
                    (x.dz.Date >= startTime && x.dz.Date < endTime) ||
                    (x.dz.Date.AddMinutes(x.gz.CzasTrwania) > startTime && x.dz.Date < endTime))
                .AnyAsync(cancellationToken);

            return !isReserved && !overlappingReservations;
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

    }
}
