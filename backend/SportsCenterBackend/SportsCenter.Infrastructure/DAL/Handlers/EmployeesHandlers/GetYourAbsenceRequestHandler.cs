using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetYourAbsenceRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetYourAbsenceRequestHandler : IRequestHandler<GetYourAbsenceRequests, IEnumerable<YourAbsenceRequestsDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetYourAbsenceRequestHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<YourAbsenceRequestsDto>> Handle(GetYourAbsenceRequests request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int trainerId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your trainer account.");
            }

            var absenceRequests = await _dbContext.BrakDostepnoscis
                .Where(b => b.PracownikId == trainerId)
                .Select(b => new YourAbsenceRequestsDto
                {
                    RequestId = b.BrakDostepnosciId,
                    Date = b.Data,
                    StartHour = b.GodzinaOd.ToTimeSpan(),
                    EndHour = b.GodzinaDo.ToTimeSpan(),
                    isApproved = b.CzyZatwierdzone
                })
                .ToListAsync(cancellationToken);

            return absenceRequests;
        }
    }
}
