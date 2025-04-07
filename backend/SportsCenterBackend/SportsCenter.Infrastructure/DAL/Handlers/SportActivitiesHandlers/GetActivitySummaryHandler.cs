using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Activities.Queries.GetActivitySummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportActivitiesHandlers
{
    internal class GetActivitySummaryHandler : IRequestHandler<GetActivitySummary, ActivitySummaryDto>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetActivitySummaryHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ActivitySummaryDto> Handle(GetActivitySummary request, CancellationToken cancellationToken)
        {

            int activityCount = await _dbContext.InstancjaZajecs
                .CountAsync(r => r.Data >= DateOnly.FromDateTime(request.StartDate) && r.Data <= DateOnly.FromDateTime(request.EndDate), cancellationToken);

            return new ActivitySummaryDto { TotalActivities = activityCount };
        }
    }
}
