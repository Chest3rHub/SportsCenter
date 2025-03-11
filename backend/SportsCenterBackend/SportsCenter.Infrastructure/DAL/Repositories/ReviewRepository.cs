using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.ClientReviewStatus;

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

        public async Task<ReviewStatus> CanUserReviewAsync(int zajeciaId, int klientId, CancellationToken cancellationToken)
        {    
            var grafikZajecKlient = await _dbContext.InstancjaZajecKlients
                .Where(gzk => gzk.KlientId == klientId)
                .Where(gzk => gzk.InstancjaZajecId ==
                              _dbContext.InstancjaZajecs
                                  .Where(iz => iz.GrafikZajec.ZajeciaId == zajeciaId)
                                  .Select(iz => iz.InstancjaZajecId)
                                  .FirstOrDefault())
                .FirstOrDefaultAsync(cancellationToken);

            if (grafikZajecKlient == null)
                return ReviewStatus.NotSignedUp;

            var instancjaZajec = await _dbContext.InstancjaZajecs
                .Where(iz => iz.InstancjaZajecId == grafikZajecKlient.InstancjaZajecId)
                .FirstOrDefaultAsync(cancellationToken);

            if (instancjaZajec == null)
                return ReviewStatus.NotSignedUp; 

            var lastPossibleDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14));

            if (instancjaZajec.Data < lastPossibleDate)
                return ReviewStatus.ReviewPeriodExpired;
         
            return ReviewStatus.CanReview;
        }
        public async Task<bool> HasUserAlreadyReviewedAsync(int zajeciaId, int klientId, CancellationToken cancellationToken)
        {
            var instancjaZajecKlientId = await _dbContext.InstancjaZajecKlients
                .Where(gzk => gzk.KlientId == klientId && gzk.InstancjaZajec.GrafikZajec.ZajeciaId == zajeciaId)
                .Select(gzk => gzk.InstancjaZajecKlientId)
                .FirstOrDefaultAsync(cancellationToken);

            if (instancjaZajecKlientId == 0)
                return false;

            var ocenaExists = await _dbContext.Ocenas
                .AnyAsync(o => o.GrafikZajecKlientId == instancjaZajecKlientId, cancellationToken);

            return ocenaExists;
        }
    }
}
