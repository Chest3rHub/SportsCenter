using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Ocena ocena, CancellationToken cancellationToken);
        Task<bool> CanUserReviewAsync(int grafikZajecKlientId, int clientId, CancellationToken cancellationToken);
        Task<bool> HasUserAlreadyReviewedAsync(int scheduleActivitiesClientId, int clientId, CancellationToken cancellationToken);
    }
}
