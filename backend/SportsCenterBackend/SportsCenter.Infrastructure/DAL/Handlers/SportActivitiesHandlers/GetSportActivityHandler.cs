using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries;
using SportsCenter.Application.Activities.Queries.GetSportActivity;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandler
{

    internal sealed class GetSportActivityHandler : IRequestHandler<GetSportActivity, SportActivityDto>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetSportActivityHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SportActivityDto> Handle(GetSportActivity request, CancellationToken cancellationToken)
        {
            var sportActivity = await _dbContext.Zajecia
           .Include(sa => sa.IdPoziomZajecNavigation)
           .Include(sa => sa.GrafikZajecs)
           .ThenInclude(g => g.Kort)
           .AsNoTracking()
           .Where(sa => sa.ZajeciaId == request.SportActivityId)
           .Select(sa => new SportActivityDto
           {
               SportActivityId = sa.ZajeciaId,
               ActivityName = sa.Nazwa,
               LevelName = sa.IdPoziomZajecNavigation.Nazwa,
               MaxParticipants = sa.GrafikZajecs.FirstOrDefault().LimitOsob,
               CostWithoutEquipment = sa.GrafikZajecs.FirstOrDefault().KosztBezSprzetu,
               CostWithEquipment = sa.GrafikZajecs.FirstOrDefault().KosztZeSprzetem,
               DayOfWeek = sa.GrafikZajecs.FirstOrDefault().DzienTygodnia,
               StartDate = sa.GrafikZajecs.FirstOrDefault().DataStartuZajec,
               StartHour = TimeOnly.FromTimeSpan(sa.GrafikZajecs.FirstOrDefault().GodzinaOd),
               DurationInMinutes = sa.GrafikZajecs.FirstOrDefault().CzasTrwania,
               CourtName = sa.GrafikZajecs.FirstOrDefault().Kort.Nazwa
           })
           .FirstOrDefaultAsync(cancellationToken);

            return sportActivity;
        }
    }
}