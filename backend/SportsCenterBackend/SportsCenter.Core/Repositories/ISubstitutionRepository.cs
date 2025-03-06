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
        Task<bool> HasEmployeeAlreadyRequestedSubstitutionForReservationAsync(int reservationId, int pracownikId);
        Task AddSubstitutionForActivitiesAsync(Zastepstwo zastepstwo);
        Task<bool> HasEmployeeAlreadyRequestedSubstitutionAsync(int zajeciaId, int pracownikId);
    }
}
