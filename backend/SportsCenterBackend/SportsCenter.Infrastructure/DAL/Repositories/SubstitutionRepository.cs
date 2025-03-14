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
        public async Task AddSubstitutionForActivitiesAsync(Zastepstwo substitution)
        {
            await _dbContext.Zastepstwos.AddAsync(substitution);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> HasEmployeeAlreadyRequestedSubstitutionAsync(int activityId, int employeeId)
        {
            return await _dbContext.Zastepstwos
                .AnyAsync(z => z.ZajeciaId == activityId && z.PracownikNieobecnyId == employeeId);
        }
        public async Task<Zastepstwo> GetSubstitutionByIdAsync(int substitutionId, CancellationToken cancellationToken)
        {
            return await _dbContext.Zastepstwos
                .Where(s => s.ZastepstwoId == substitutionId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task UpdateSubstitutionAsync(int substitutionId, int substituteEmployeeId, int approvedEmployeeId, CancellationToken cancellationToken)
        {
            var substitution = await _dbContext.Zastepstwos
                .Where(s => s.ZastepstwoId == substitutionId)
                .FirstOrDefaultAsync(cancellationToken);

            substitution.PracownikZastepujacyId = substituteEmployeeId;
            substitution.PracownikZatwierdzajacyId = approvedEmployeeId;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
