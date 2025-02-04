using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface IReservationRepository
    {
        Task AddReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken);

        //co do tych dwóch metod ponizej to nie jestem pewna czy nie zrobic dla nich osobnych
        //repozytoriow ale na razie je tu umieszczam zeby nie rozdrabniac za bardzo projektu
        Task<bool> IsCourtAvailableAsync(int courtId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
        Task<bool> IsTrainerAvailableAsync(int trainerId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
        Task<Rezerwacja> GetReservationByIdAsync(int reservationId, CancellationToken cancellationToken);
        Task UpdateReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken);
        Task DeleteReservationAsync(Rezerwacja reservation, CancellationToken cancellationToken);
        Task<IEnumerable<Kort>> GetAvailableCourtsAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
        Task<IEnumerable<Pracownik>> GetAvailableTrainersAsync(DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
    }
}
