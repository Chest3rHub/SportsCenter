using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SportsCenter.Core.Enums.ClientReviewStatus;

namespace SportsCenter.Core.Repositories
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Ocena ocena, CancellationToken cancellationToken);
        Task<ReviewStatus> CanUserReviewAsync(int zajeciaId, int klientId, CancellationToken cancellationToken);
        Task<bool> HasUserAlreadyReviewedAsync(int scheduleActivitiesClientId, int clientId, CancellationToken cancellationToken);
    }
}
