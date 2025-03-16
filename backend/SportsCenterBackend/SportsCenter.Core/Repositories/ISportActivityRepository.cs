using Microsoft.AspNetCore.Mvc;
using SportsCenter.Core.Entities;
using Stripe.Forwarding;
namespace SportsCenter.Core.Repositories;

public interface ISportActivityRepository
{
    Task AddSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken);
    Task AddScheduleAsync(GrafikZajec schedule, CancellationToken cancellationToken);
    Task<Zajecium> GetSportActivityByIdAsync(int sportActivityId, CancellationToken cancellationToken);
    Task<IEnumerable<Zajecium>> GetAllSportActivitiesAsync(CancellationToken cancellationToken);
    Task RemoveSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken);
    Task<IEnumerable<GrafikZajec>> GetSchedulesByTrainerIdAsync(int trainerId, CancellationToken cancellationToken);
    Task<(DateTime? date, int? duration)> GetActivityDetailsByIdAsync(int activitiesId);
    Task<bool> IsTrainerAssignedToActivityAsync(int activityId, int trainerId);
    Task<(DateTime date, TimeSpan startTime, TimeSpan endTime)?> GetActivityDetailsAsync(int activityId, CancellationToken cancellationToken);
    Task<int> EnsureLevelNameExistsAsync(string levelName, CancellationToken cancellationToken);
    Task<InstancjaZajec> GetInstanceByScheduleAndDateAsync(GrafikZajec activitySchedule, DateOnly selectedDate, CancellationToken cancellationToken);
    Task AddInstanceAsync(InstancjaZajec instatnionOfActivity, CancellationToken cancellationToken);
    Task<GrafikZajec?> GetScheduleByActivityIdAsync(int activityId, CancellationToken cancellationToken);
    Task<bool> IsClientSignedUpAsync(int clientId, int instatnionOfActivity, CancellationToken cancellationToken);
    Task AddClientToInstanceAsync(InstancjaZajecKlient signUp, CancellationToken cancellationToken);
    Task<int> CancelInstanceOfActivityAsync(int instatnceOfActivity, CancellationToken cancellationToken);
    Task<InstancjaZajec> GetInstanceOfActivityAsync(int instatnceOfActivity, CancellationToken cancellationToken);

}