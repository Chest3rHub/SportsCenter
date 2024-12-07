using SportsCenter.Core.Entities;
namespace SportsCenter.Core.Repositories;

public interface ISportActivityRepository
{ 
    Task<int> AddSportActivityAsync(SportActivity sportActivity, CancellationToken cancellationToken);
    Task AddScheduleAsync(SportActivitySchedule schedule, CancellationToken cancellationToken);
    Task<SportActivity> GetSportActivityByIdAsync(int sportActivityId, CancellationToken cancellationToken); 
    Task<IEnumerable<SportActivity>> GetAllSportActivitiesAsync(CancellationToken cancellationToken);
    Task RemoveSportActivityAsync(SportActivity sportActivity, CancellationToken cancellationToken);
}

