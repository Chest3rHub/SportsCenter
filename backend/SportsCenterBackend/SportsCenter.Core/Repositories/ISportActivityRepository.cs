using SportsCenter.Core.Entities;
namespace SportsCenter.Core.Repositories;

public interface ISportActivityRepository
{ 
    Task<int> AddSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken);
    Task AddScheduleAsync(GrafikZajec schedule, CancellationToken cancellationToken);
    Task<Zajecium> GetSportActivityByIdAsync(int sportActivityId, CancellationToken cancellationToken); 
    Task<IEnumerable<Zajecium>> GetAllSportActivitiesAsync(CancellationToken cancellationToken);
    Task RemoveSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken);
    Task<IEnumerable<GrafikZajec>> GetSchedulesByTrainerIdAsync(int trainerId, CancellationToken cancellationToken);
}

