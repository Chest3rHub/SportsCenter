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
        public async Task AddReviewAsync(Ocena review, CancellationToken cancellationToken)
        {
            _dbContext.Ocenas.AddAsync(review, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ReviewStatus> CanUserReviewAsync(int instanceOfActivityId, int clientId, CancellationToken cancellationToken)
        {    
            var instantionOfActivityClient = await _dbContext.InstancjaZajecKlients
                .Where(gzk => gzk.KlientId == clientId)
                .Where(gzk => gzk.InstancjaZajecId == instanceOfActivityId)
                .FirstOrDefaultAsync(cancellationToken);

            if (instantionOfActivityClient == null)
                return ReviewStatus.NotSignedUp;

            var instantionOfActivity = await _dbContext.InstancjaZajecs
                .Where(iz => iz.InstancjaZajecId == instantionOfActivityClient.InstancjaZajecId)
                .FirstOrDefaultAsync(cancellationToken);

            if (instantionOfActivity == null)
                return ReviewStatus.NotSignedUp; 

            var lastPossibleDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-14));

            if (instantionOfActivity.Data < lastPossibleDate)
                return ReviewStatus.ReviewPeriodExpired;
         
            return ReviewStatus.CanReview;
        }
        public async Task<bool> HasUserAlreadyReviewedAsync(int instanceOfActivityId, int clientId, CancellationToken cancellationToken)
        {
            var instantionOfActivityClient = await _dbContext.InstancjaZajecKlients
                .Where(gzk => gzk.KlientId == clientId && gzk.InstancjaZajecId == instanceOfActivityId)
                .Select(gzk => gzk.InstancjaZajecKlientId)
                .FirstOrDefaultAsync(cancellationToken);

            if (instantionOfActivityClient == 0)
                return false;

            var reviewExists = await _dbContext.Ocenas
                .AnyAsync(o => o.InstancjaZajecKlientId == instantionOfActivityClient, cancellationToken);

            return reviewExists;
        }
    }
}
