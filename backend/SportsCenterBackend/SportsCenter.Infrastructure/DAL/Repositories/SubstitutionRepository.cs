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
    public class SubstitutionRepository : ISubstitutionRepository
    {
        private SportsCenterDbContext _dbContext;

        public SubstitutionRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddSubstitutionForReservationAsync(Zastepstwo substitution)
        {
            await _dbContext.Zastepstwos.AddAsync(substitution);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> HasEmployeeAlreadyRequestedSubstitutionForReservationAsync(int reservationId, int pracownikId)
        {
            return await _dbContext.Zastepstwos
                .AnyAsync(z => z.RezerwacjaId == reservationId && z.PracownikNieobecnyId == pracownikId);
        }
        public async Task AddSubstitutionForActivitiesAsync(Zastepstwo zastepstwo)
        {
            await _dbContext.Zastepstwos.AddAsync(zastepstwo);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> HasEmployeeAlreadyRequestedSubstitutionAsync(int zajeciaId, int pracownikId)
        {
            return await _dbContext.Zastepstwos
                .AnyAsync(z => z.ZajeciaId == zajeciaId && z.PracownikNieobecnyId == pracownikId);
        }
    }
}
