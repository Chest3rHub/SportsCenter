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
        int PageSize = 6;
        int NumberPerPage = 7;

            var absenceRequests = await _dbContext.BrakDostepnoscis
              .Where(b => !b.CzyZatwierdzone)
              .OrderByDescending(b => b.Data) //Sortowane od najnowszych poki co
              .Skip(request.Offset * PageSize)
              .Take(NumberPerPage)
              .Select(b => new AbsenceRequestDto
              {
                  RequestId = b.BrakDostepnosciId,
                  Date = b.Data,
                  StartHour = b.GodzinaOd.ToTimeSpan(),
                  EndHour = b.GodzinaDo.ToTimeSpan(),
                  EmployeeId = b.PracownikId
              })
              .AsNoTracking()
              .ToListAsync(cancellationToken);

            return absenceRequests;
        }
    }
}
