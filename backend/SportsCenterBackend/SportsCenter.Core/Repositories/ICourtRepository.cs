using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface ICourtRepository
    {
        Task<bool> IsCourtAvailableAsync(int courtId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken, int? reservationIdToExclude = null);
        Task<IEnumerable<Kort>> GetAvailableCourtsAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
        Task<bool> CheckIfCourtExists(string courtName, CancellationToken cancellationToken);
        Task<int?> GetCourtIdByName(string courtName, CancellationToken cancellationToken);
    }
}
