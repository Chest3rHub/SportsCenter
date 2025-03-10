using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Repositories;

public class SportActivityRepository : ISportActivityRepository
{
    private SportsCenterDbContext _dbContext;

    public SportActivityRepository(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken = default)
    {
        _dbContext.Zajecia.Add(sportActivity);
        await _dbContext.SaveChangesAsync();
        return sportActivity.ZajeciaId;
    }

    public async Task AddScheduleAsync(GrafikZajec schedule, CancellationToken cancellationToken = default)
    {
        _dbContext.GrafikZajecs.Add(schedule);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Zajecium> GetSportActivityByIdAsync(int sportActivityId, CancellationToken cancellationToken)
    {
        return await _dbContext.Zajecia
            .Include(sa => sa.IdPoziomZajecNavigation)
            .Include(sa => sa.GrafikZajecs)
            .FirstOrDefaultAsync(sa => sa.IdPoziomZajec == sportActivityId, cancellationToken);
    }
    public async Task<IEnumerable<Zajecium>> GetAllSportActivitiesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Zajecia
            .Include(sa => sa.IdPoziomZajec)
            .Include(sa => sa.GrafikZajecs)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task RemoveSportActivityAsync(Zajecium sportActivity, CancellationToken cancellationToken)
    {
        _dbContext.Zajecia.Remove(sportActivity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<GrafikZajec>> GetSchedulesByTrainerIdAsync(int trainerId, CancellationToken cancellationToken)
    {
        return await _dbContext.GrafikZajecs
            .Where(gz => gz.PracownikId == trainerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<(DateTime? date, int? duration)> GetActivityDetailsByIdAsync(int activitiesId)
    {
        var activityDetails = await _dbContext.GrafikZajecs
            .Where(gz => gz.ZajeciaId == activitiesId)
            .Select(gz => new
            {
                gz.CzasTrwania,
                gz.DzienTygodnia,
                gz.GodzinaOd
            })
            .FirstOrDefaultAsync();

        if (activityDetails == null)
        {
            return (null, null);
        }

        string dzienTygodnia = activityDetails.DzienTygodnia;
        TimeSpan godzinaOd = activityDetails.GodzinaOd;

        var currentDate = DateTime.Now;

        var dayOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
            .FirstOrDefault(d => d.ToString() == dzienTygodnia);

        DateTime activityDate = currentDate.AddDays((int)dayOfWeek - (int)currentDate.DayOfWeek);
        if ((int)dayOfWeek <= (int)currentDate.DayOfWeek)
        {
            activityDate = activityDate.AddDays(7);
        }

        DateTime finalDate = activityDate.Add(godzinaOd);

        return (finalDate, activityDetails.CzasTrwania);
    }
    public async Task<bool> IsTrainerAssignedToActivityAsync(int activityId, int trainerId)
    {
        return await _dbContext.GrafikZajecs
            .Where(gz => gz.ZajeciaId == activityId && gz.PracownikId == trainerId)
            .AnyAsync();
    }
    public async Task<(DateTime date, TimeSpan startTime, TimeSpan endTime)?> GetActivityDetailsAsync(int activityId, CancellationToken cancellationToken)
    {

        var activity = await _dbContext.GrafikZajecs
            .Where(g => g.ZajeciaId == activityId)
            .Select(g => new { g.DzienTygodnia, g.GodzinaOd, g.CzasTrwania })
            .FirstOrDefaultAsync(cancellationToken);

        if (activity == null)
            return null;

        var currentDate = DateTime.Now;

        var dayOfWeek = Enum.GetValues(typeof(DayOfWeek))
            .Cast<DayOfWeek>()
            .FirstOrDefault(d => d.ToString() == activity.DzienTygodnia);

        DateTime activityDate = currentDate.AddDays((int)dayOfWeek - (int)currentDate.DayOfWeek);

        if ((int)dayOfWeek <= (int)currentDate.DayOfWeek)
        {
            activityDate = activityDate.AddDays(7);
        }

        TimeSpan startTime = activityDate.Date.Add(activity.GodzinaOd).TimeOfDay;
        TimeSpan endTime = startTime + TimeSpan.FromMinutes(activity.CzasTrwania);

        return (activityDate.Date, startTime, endTime);
    }
}