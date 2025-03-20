using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries.GetAllSportActivities;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandler
{

    internal sealed class GetAllSportActivitiesHandler : IRequestHandler<GetAllSportActivities, IEnumerable<SportActivityDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetAllSportActivitiesHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SportActivityDto>> Handle(GetAllSportActivities request, CancellationToken cancellationToken)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            DateOnly startOfWeek = today.AddDays(-(int)today.DayOfWeek + 1).AddDays(request.WeekOffset * 7);
            DateOnly endOfWeek = startOfWeek.AddDays(6);

            var weeklyActivities = await _dbContext.GrafikZajecs
                .Include(g => g.Zajecia)
                .ThenInclude(z => z.IdPoziomZajecNavigation)
                .Include(g => g.Kort)
                .ToListAsync(cancellationToken);

            var existingInstances = await _dbContext.InstancjaZajecs
                .Where(i => i.Data >= startOfWeek && i.Data <= endOfWeek)
                .ToListAsync(cancellationToken);

            var result = new List<SportActivityDto>();

            foreach (var activity in weeklyActivities)
            {
                for (int i = 0; i < 7; i++)
                {
                    DateOnly activityDate = startOfWeek.AddDays(i);

                    if ((int)activityDate.DayOfWeek != DzienTygodniaNaNumer(activity.DzienTygodnia)) continue;

                    var existingInstance = existingInstances.FirstOrDefault(x => x.GrafikZajecId == activity.GrafikZajecId && x.Data == activityDate);
                    bool isCanceled = existingInstance?.CzyOdwolane ?? false;

                    result.Add(new SportActivityDto
                    {
                        SportActivityId = activity.ZajeciaId,
                        ActivityName = activity.Zajecia.Nazwa,
                        LevelName = activity.Zajecia.IdPoziomZajecNavigation.Nazwa,
                        ActivityDate = activityDate,
                        StartDate = activity.DataStartuZajec,
                        DayOfWeek = activity.DzienTygodnia,
                        StartHour = TimeOnly.FromTimeSpan(activity.GodzinaOd),
                        DurationInMinutes = activity.CzasTrwania,
                        CourtName = activity.Kort.Nazwa,
                        MaxParticipants = 10,
                        CostWithoutEquipment = 0,
                        CostWithEquipment = 0,
                        isCanleced = isCanceled
                    });              
                }
            }
            return result;
        }
        private int DzienTygodniaNaNumer(string dzien)
        {
            return dzien.ToLower() switch
            {
                "poniedzialek" => 1,
                "wtorek" => 2,
                "sroda" => 3,
                "czwartek" => 4,
                "piatek" => 5,
                "sobota" => 6,
                "niedziela" => 0,
                _ => -1
            };
        }
    }
}
