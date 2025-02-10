using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.SportActivitiesException;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandler;

internal sealed class GetSportActivityByIdQueryHandler : IRequestHandler<GetSportActivityById, SportActivityDto>
{
    private readonly SportsCenterDbContext _dbContext;

    public GetSportActivityByIdQueryHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SportActivityDto> Handle(GetSportActivityById request, CancellationToken cancellationToken)
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
                Name = sa.Nazwa,
                Level = sa.IdPoziomZajec,
                MaxParticipants = sa.GrafikZajecs.FirstOrDefault().LimitOsob,
                CostWithoutEquipment = sa.GrafikZajecs.FirstOrDefault().KoszBezSprzetu,
                CostWithEquipment = sa.GrafikZajecs.FirstOrDefault().KoszZeSprzetem 
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (sportActivity == null)
        {
            throw new SportActivityNotFoundException(sportActivity.SportActivityId);
        }

        return sportActivity;
    }
}