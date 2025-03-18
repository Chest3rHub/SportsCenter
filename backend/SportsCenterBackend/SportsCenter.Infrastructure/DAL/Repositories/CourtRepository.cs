using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    public class CourtRepository : ICourtRepository
    {
        private SportsCenterDbContext _dbContext;

        public CourtRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> IsCourtAvailableAsync(int courtId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
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
         
            string dayOfWeek = dniTygodnia[startTime.DayOfWeek];

            double startHourInMinutes = startTime.TimeOfDay.TotalMinutes;
            double endHourInMinutes = endTime.TimeOfDay.TotalMinutes;

            bool isReserved = await _dbContext.Rezerwacjas
                .AnyAsync(r => r.KortId == courtId &&
                               r.DataOd < endTime &&
                               r.DataDo > startTime &&
                               r.CzyOdwolana == false, cancellationToken);

            var activity = await _dbContext.GrafikZajecs
                .Where(gz => gz.KortId == courtId && gz.DzienTygodnia == dayOfWeek)
                .ToListAsync(cancellationToken);
     
            bool hasOverlappingClasses = activity.Any(gz =>
            {
                double zajeciaEndMinutes = gz.GodzinaOd.TotalMinutes + gz.CzasTrwania; // czas zakończenia zajęć

                return (gz.GodzinaOd.TotalMinutes < endHourInMinutes) &&
                       (zajeciaEndMinutes > startHourInMinutes);
            });

            return !isReserved && !hasOverlappingClasses;
        }

        public async Task<IEnumerable<Kort>> GetAvailableCourtsAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
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

            string dayOfWeek = dniTygodnia[startTime.DayOfWeek];
            TimeSpan startHour = startTime.TimeOfDay;
            TimeSpan endHour = endTime.TimeOfDay;

            var availableCourts = await _dbContext.Korts
        .Where(c => !_dbContext.Rezerwacjas
            .Any(r => r.KortId == c.KortId &&
                      r.DataOd < endTime &&
                      r.DataDo > startTime &&
                      r.CzyOdwolana == false))
        .ToListAsync(cancellationToken);

            availableCourts = availableCourts
                .Where(c => !_dbContext.GrafikZajecs
                    .Where(gz => gz.KortId == c.KortId &&
                                 gz.DzienTygodnia == dayOfWeek)
                    .AsEnumerable()
                    .Any(gz =>
                    {
                        TimeSpan godzinaZakonczeniaZajec = gz.GodzinaOd.Add(TimeSpan.FromMinutes(gz.CzasTrwania));

                        return (gz.GodzinaOd < endHour) && (godzinaZakonczeniaZajec > startHour);
                    }))
                .ToList();

            return availableCourts;
        }
        public async Task<bool> CheckIfCourtExists(string courtName, CancellationToken cancellationToken)
        {
            return await _dbContext.Korts.AnyAsync(k => k.Nazwa == courtName, cancellationToken);
        }
        public async Task<int?> GetCourtIdByName(string courtName, CancellationToken cancellationToken)
        {
            return await _dbContext.Korts
                .Where(k => k.Nazwa == courtName)
                .Select(k => (int?)k.KortId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
