using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries.GetAllSportActivities;
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
            return await _dbContext.Zajecia
                .Include(sa => sa.IdPoziomZajecNavigation)
                .Include(sa => sa.GrafikZajecs)
                .ThenInclude(g => g.Kort)
                .AsNoTracking()
                .Select(sa => new SportActivityDto
                {
                    SportActivityId = sa.ZajeciaId,
                    ActivityName = sa.Nazwa,
                    LevelName = sa.IdPoziomZajecNavigation.Nazwa,
                    MaxParticipants = sa.GrafikZajecs.FirstOrDefault().LimitOsob,
                    CostWithoutEquipment = sa.GrafikZajecs.FirstOrDefault().KosztBezSprzetu,
                    CostWithEquipment = sa.GrafikZajecs.FirstOrDefault().KosztZeSprzetem,
                    DayOfWeek = sa.GrafikZajecs.FirstOrDefault().DzienTygodnia,
                    StartHour = TimeOnly.FromTimeSpan(sa.GrafikZajecs.FirstOrDefault().GodzinaOd),
                    DurationInMinutes = sa.GrafikZajecs.FirstOrDefault().CzasTrwania,
                    CourtName = sa.GrafikZajecs.FirstOrDefault().Kort.Nazwa
                })
                .ToListAsync(cancellationToken);
        }
    }
}
