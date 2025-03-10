using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandler;

internal sealed class GetSportActivitiesQueryHandler : IRequestHandler<GetSportActivities, IEnumerable<SportActivityDto>>
{
    private readonly SportsCenterDbContext _dbContext;

    public GetSportActivitiesQueryHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<SportActivityDto>> Handle(GetSportActivities request, CancellationToken cancellationToken)
    {
        return await _dbContext.Zajecia
            .Include(sa => sa.IdPoziomZajecNavigation)
            .Include(sa => sa.GrafikZajecs)
            .ThenInclude(g => g.Kort)
            .AsNoTracking()
            .Select(sa => new SportActivityDto
            {
                SportActivityId = sa.ZajeciaId,
                Name = sa.Nazwa,
                Level = sa.IdPoziomZajec,
                MaxParticipants = sa.GrafikZajecs.FirstOrDefault().LimitOsob, 
                CostWithoutEquipment = sa.GrafikZajecs.FirstOrDefault().KosztBezSprzetu,
                CostWithEquipment = sa.GrafikZajecs.FirstOrDefault().KosztZeSprzetem
            })
            .ToListAsync(cancellationToken);
    }
}