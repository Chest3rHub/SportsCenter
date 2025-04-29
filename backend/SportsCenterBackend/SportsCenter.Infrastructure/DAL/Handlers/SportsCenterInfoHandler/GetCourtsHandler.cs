using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetTrainers;
using SportsCenter.Application.SportsCenterManagement.Queries.GetCourts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SportsCenterInfoHandler
{
    internal sealed class GetCourtsHandler : IRequestHandler<GetCourts, IEnumerable<CourtDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetCourtsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<CourtDto>> Handle(GetCourts request, CancellationToken cancellationToken)
        {
            return await _dbContext.Korts
             .Select(k => new CourtDto
             {
                 Id = k.KortId,
                 Name = k.Nazwa,
             })
             .AsNoTracking()
             .ToListAsync(cancellationToken);
        }
    }
}
