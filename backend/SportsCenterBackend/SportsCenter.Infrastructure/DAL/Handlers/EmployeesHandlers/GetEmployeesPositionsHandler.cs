using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetEmployeesPositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetEmployeesPositionsHandler : IRequestHandler<GetEmployeesPositions, IEnumerable<EmployeePositionDto>>
    {

        private readonly SportsCenterDbContext _dbContext;

        public GetEmployeesPositionsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<EmployeePositionDto>> Handle(GetEmployeesPositions request, CancellationToken cancellationToken)
        {
            return await _dbContext.TypPracownikas
                .Where(ep => ep.Nazwa != "Wlasciciel")
                .Select(ep => new EmployeePositionDto
                {
                    Id = ep.IdTypPracownika,
                    PositionName = ep.Nazwa
                })
                .ToListAsync(cancellationToken);
        }
    }
}
