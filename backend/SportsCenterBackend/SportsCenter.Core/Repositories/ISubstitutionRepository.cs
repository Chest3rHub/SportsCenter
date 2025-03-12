using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface ISubstitutionRepository
    {
        Task AddSubstitutionForReservationAsync(Zastepstwo substitution);
        Task<bool> HasEmployeeAlreadyRequestedSubstitutionForReservationAsync(int reservationId, int employeeId);
        Task AddSubstitutionForActivitiesAsync(Zastepstwo substitution);
        Task<bool> HasEmployeeAlreadyRequestedSubstitutionAsync(int activityId, int employeeId);
        Task<Zastepstwo> GetSubstitutionByIdAsync(int substitutionId, CancellationToken cancellationToken);
        Task UpdateSubstitutionAsync(int substitutionId, int substituteEmployeeId, int approvedEmployeeId, CancellationToken cancellationToken);
    }
}
