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
            string dzienTygodnia = startTime.ToString("dddd", new System.Globalization.CultureInfo("pl-PL"));
            double godzinaOdMinutes = startTime.TimeOfDay.TotalMinutes;
            double godzinaDoMinutes = endTime.TimeOfDay.TotalMinutes;

            bool isReserved = await _dbContext.Rezerwacjas
                .AnyAsync(r => r.KortId == courtId &&
                               r.DataOd < endTime &&
                               r.DataDo > startTime, cancellationToken);

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

        public async Task<IEnumerable<Kort>> GetAvailableCourtsAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
        {
            string dzienTygodnia = startTime.ToString("dddd", new System.Globalization.CultureInfo("pl-PL"));
            TimeSpan godzinaOd = startTime.TimeOfDay;
            TimeSpan godzinaDo = endTime.TimeOfDay;

            var availableCourts = await _dbContext.Korts
                .Where(c => !_dbContext.Rezerwacjas
                    .Any(r => r.KortId == c.KortId &&
                              r.DataOd < endTime &&
                              r.DataDo > startTime))
                .ToListAsync(cancellationToken);

            availableCourts = availableCourts
                .Where(c => !_dbContext.GrafikZajecs
                    .Where(gz => gz.KortId == c.KortId && gz.DzienTygodnia == dzienTygodnia)
                    .AsEnumerable()
                    .Any(gz =>
                    {
                        TimeSpan godzinaZakonczeniaZajec = gz.GodzinaOd.Add(TimeSpan.FromMinutes(gz.CzasTrwania));

                        return (gz.GodzinaOd < godzinaDo) && (godzinaZakonczeniaZajec > godzinaOd);
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
