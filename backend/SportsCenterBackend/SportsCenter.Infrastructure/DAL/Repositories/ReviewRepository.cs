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
    public class ReviewRepository : IReviewRepository
    {
        private SportsCenterDbContext _dbContext;

        public ReviewRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddReviewAsync(Ocena ocena, CancellationToken cancellationToken)
        {
            _dbContext.Ocenas.AddAsync(ocena, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> CanUserReviewAsync(int grafikZajecKlientId, int clientId, CancellationToken cancellationToken)
        {
            var grafikZajecKlient = await _dbContext.InstancjaZajecKlients
                .FirstOrDefaultAsync(gzk => gzk.InstancjaZajecKlientId == grafikZajecKlientId && gzk.KlientId == clientId);

            if (grafikZajecKlient == null)
                return false;

            var lastPossibleDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14));

            return grafikZajecKlient.DataZapisu >= lastPossibleDate;
        }
        public async Task<bool> HasUserAlreadyReviewedAsync(int scheduleActivitiesClientId, int clientId, CancellationToken cancellationToken)
        {           
            return await _dbContext.Ocenas
                .AnyAsync(o => o.GrafikZajecKlientId == scheduleActivitiesClientId && o.InstancjaZajecKlient.KlientId == clientId, cancellationToken);
        }
    }
}
