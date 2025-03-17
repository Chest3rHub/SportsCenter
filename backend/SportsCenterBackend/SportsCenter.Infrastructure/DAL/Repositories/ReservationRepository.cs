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
            string dzienTygodnia = startTime.ToString("dddd", new System.Globalization.CultureInfo("pl-PL"));
            double godzinaOdMinutes = startTime.TimeOfDay.TotalMinutes;
            double godzinaDoMinutes = endTime.TimeOfDay.TotalMinutes;

            bool isReserved = await _dbContext.Rezerwacjas
                .AnyAsync(r => r.KortId == courtId &&
                               r.DataOd < endTime &&
                               r.DataDo > startTime &&
                                r.CzyOdwolana == false, cancellationToken);

            var zajecia = await _dbContext.GrafikZajecs
                .Where(gz => gz.KortId == courtId && gz.DzienTygodnia == dzienTygodnia)
                .ToListAsync(cancellationToken);

            bool hasOverlappingClasses = zajecia.Any(gz =>
            {
                double zajeciaEndMinutes = gz.GodzinaOd.TotalMinutes + (gz.CzasTrwania * 60);

                return (gz.GodzinaOd.TotalMinutes < godzinaDoMinutes) &&
                       (zajeciaEndMinutes > godzinaOdMinutes);
            });

            return !isReserved && !hasOverlappingClasses;
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
        
        //public async Task<IEnumerable<Kort>> GetAvailableCourtsAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        //{
        //    string dzienTygodnia = startTime.ToString("dddd", new System.Globalization.CultureInfo("pl-PL"));
        //    TimeSpan godzinaOd = startTime.TimeOfDay;
        //    TimeSpan godzinaDo = endTime.TimeOfDay;

        //    var availableCourts = await _dbContext.Korts
        //        .Where(c => !_dbContext.Rezerwacjas
        //            .Any(r => r.KortId == c.KortId &&
        //                      r.DataOd < endTime &&
        //                      r.DataDo > startTime &&
        //                      r.CzyOdwolana == false))
        //        .ToListAsync(cancellationToken);

        //    availableCourts = availableCourts
        //        .Where(c => !_dbContext.GrafikZajecs
        //            .Where(gz => gz.KortId == c.KortId && gz.DzienTygodnia == dzienTygodnia)
        //            .AsEnumerable()
        //            .Any(gz =>
        //            {
        //                TimeSpan godzinaZakonczeniaZajec = gz.GodzinaOd.Add(TimeSpan.FromMinutes(gz.CzasTrwania));

        //                return (gz.GodzinaOd < godzinaDo) && (godzinaZakonczeniaZajec > godzinaOd);
        //            }))
        //        .ToList();

        //    return availableCourts;
        //}

        public async Task<IEnumerable<Kort>> GetAvailableCourtsAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
            string dzienTygodnia = startTime.ToString("dddd", new System.Globalization.CultureInfo("pl-PL"));
            TimeSpan godzinaOd = startTime.TimeOfDay;
            TimeSpan godzinaDo = endTime.TimeOfDay;

            var availableCourts = await _dbContext.Korts
                .Where(c => !_dbContext.Rezerwacjas
                    .Any(r => r.KortId == c.KortId &&
                              r.DataOd < endTime &&
                              r.DataDo > startTime &&
                              r.CzyOdwolana == false)) 
                .Where(c => !_dbContext.GrafikZajecs
                    .Any(gz => gz.KortId == c.KortId &&
                               gz.DzienTygodnia == dzienTygodnia &&
                               (gz.GodzinaOd < godzinaDo && (gz.GodzinaOd.Add(TimeSpan.FromMinutes(gz.CzasTrwania)) > godzinaOd))))
                .ToListAsync(cancellationToken);

            return availableCourts;
        }

        public async Task<IEnumerable<Pracownik>> GetAvailableTrainersAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
            string dzienTygodnia = startTime.ToString("dddd", new System.Globalization.CultureInfo("pl-PL"));
            TimeSpan godzinaOd = startTime.TimeOfDay;
            TimeSpan godzinaDo = endTime.TimeOfDay;

            var availableTrainers = await _dbContext.Pracowniks
                .Where(t => !_dbContext.Rezerwacjas
                    .Any(r => r.TrenerId == t.PracownikId &&
                        r.DataOd < endTime &&
                        r.DataDo > startTime &&
                      r.CzyOdwolana == false))
                .Join(
                    _dbContext.TypPracownikas,
                    t => t.IdTypPracownika,
                    tp => tp.IdTypPracownika,
                    (t, tp) => new { t, tp })
                    .Where(joined => joined.tp.Nazwa == "Trener")
                    .Select(joined => joined.t)
                    .ToListAsync(cancellationToken);

            availableTrainers = availableTrainers
                .Where(t => !_dbContext.GrafikZajecs
                    .Where(gz => gz.PracownikId == t.PracownikId && gz.DzienTygodnia == dzienTygodnia)
                    .AsEnumerable()
                    .Any(gz =>
                    {
                        TimeSpan godzinaZakonczeniaZajec = gz.GodzinaOd.Add(TimeSpan.FromMinutes(gz.CzasTrwania));

                        return (gz.GodzinaOd < godzinaDo) && (godzinaZakonczeniaZajec > godzinaOd);
                    }))
                .ToList();

            return availableTrainers;
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
    }
}
