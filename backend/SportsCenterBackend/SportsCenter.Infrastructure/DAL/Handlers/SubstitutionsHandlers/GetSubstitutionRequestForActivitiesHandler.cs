using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Substitutions.Queries.GetSubstitutionRequestsForActivities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.SubstitutionsHandlers
{
    internal sealed class GetSubstitutionRequestForActivitiesHandler : IRequestHandler<GetSubstitutionRequests, IEnumerable<SubstitutionRequestDto>>
    {
        private readonly SportsCenterDbContext _dbContext;    

        public GetSubstitutionRequestForActivitiesHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<SubstitutionRequestDto>> Handle(GetSubstitutionRequests request, CancellationToken cancellationToken)
        {
            int pageSize = 6;
            int numberPerPage = 7;

            var substitutionRequests = await _dbContext.Zastepstwos
                .Where(z => z.PracownikZastepujacy == null)
                .Select(z => new SubstitutionRequestDto
                {
                    SubstitutionId = z.ZastepstwoId,
                    Date = z.Data,
                    StartHour = z.GodzinaOd.ToTimeSpan(),
                    EndHour = z.GodzinaDo.ToTimeSpan(),
                    ActivityId = z.ZajeciaId,
                    ReservationId = z.RezerwacjaId,
                    EmployeeId = z.PracownikNieobecnyId
                })
                 .Skip(request.Offset * pageSize)
                .Take(numberPerPage)
                .ToListAsync(cancellationToken);

            return substitutionRequests;
        }
    }
}
