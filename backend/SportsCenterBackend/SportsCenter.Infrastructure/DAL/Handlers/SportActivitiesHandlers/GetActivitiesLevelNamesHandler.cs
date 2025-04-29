using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries.GetActivitiesLevelNames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandlers
{
    internal sealed class GetActivitiesLevelNamesHandler : IRequestHandler<GetActivitiesLevelNames, IEnumerable<ActivityLevelNameDto>>
    {

        private readonly SportsCenterDbContext _dbContext;

        public GetActivitiesLevelNamesHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ActivityLevelNameDto>> Handle(GetActivitiesLevelNames request, CancellationToken cancellationToken)
        {
            return await _dbContext.PoziomZajecs
              .Select(p => new ActivityLevelNameDto
              {
                  LevelId = p.IdPoziomZajec,
                  LevelName = p.Nazwa,
              })
              .AsNoTracking()
              .ToListAsync(cancellationToken);
        }
    }
}
