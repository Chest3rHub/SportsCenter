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

        int pageSize = 6;
        int numberPerPage = 7;

        public GetAllSportActivitiesHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SportActivityDto>> Handle(GetAllSportActivities request, CancellationToken cancellationToken)
        {
            var activities = await _dbContext.GrafikZajecs
                .Include(g => g.Zajecia)
                    .ThenInclude(z => z.IdPoziomZajecNavigation)
                .Include(g => g.Kort)
                .OrderBy(g => g.Zajecia.ZajeciaId)
                .ThenBy(g => g.GodzinaOd)
                .Skip(request.Offset * pageSize)
                .Take(numberPerPage)
                .ToListAsync(cancellationToken);

            var result = activities.Select(activity => new SportActivityDto
            {
                SportActivityId = activity.ZajeciaId,
                ActivityName = activity.Zajecia.Nazwa,
                LevelName = activity.Zajecia.IdPoziomZajecNavigation.Nazwa,
                StartDate = activity.DataStartuZajec,
                DayOfWeek = activity.DzienTygodnia,
                StartHour = TimeOnly.FromTimeSpan(activity.GodzinaOd),
                DurationInMinutes = activity.CzasTrwania,
                CourtName = activity.Kort.Nazwa,
                MaxParticipants = 10,
                CostWithoutEquipment = 0,
                CostWithEquipment = 0,
                isCanleced = false
            });

            return result.ToList();
        }
    }
}
