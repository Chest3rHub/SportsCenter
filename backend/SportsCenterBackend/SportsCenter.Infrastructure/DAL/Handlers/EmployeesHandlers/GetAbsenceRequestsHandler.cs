using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetAbsenceRequest;
using SportsCenter.Application.Employees.Queries.GetAbsenceRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetAbsenceRequestsHandler : IRequestHandler<GetAbsenceRequests, IEnumerable<AbsenceRequestDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetAbsenceRequestsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AbsenceRequestDto>> Handle(GetAbsenceRequests request, CancellationToken cancellationToken)
        {
            var absenceRequests = await _dbContext.BrakDostepnoscis
               .Where(b => !b.CzyZatwierdzone)
               .Select(b => new AbsenceRequestDto
               {
                   RequestId = b.BrakDostepnosciId,
                   Date = b.Data,
                   StartHour = b.GodzinaOd.ToTimeSpan(),
                   EndHour = b.GodzinaDo.ToTimeSpan(),
                   EmployeeId = b.PracownikId
               })
               .ToListAsync(cancellationToken);

            return absenceRequests;
        }
    }
}
