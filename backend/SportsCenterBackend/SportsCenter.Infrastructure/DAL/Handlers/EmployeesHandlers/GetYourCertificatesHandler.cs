using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetYourCertificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetYourCertificatesHandler : IRequestHandler<GetYourCertificates, IEnumerable<YourCertificatesDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetYourCertificatesHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<YourCertificatesDto>> Handle(GetYourCertificates request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int trainerId))
            {
                throw new UnauthorizedAccessException("You cannot access certificates without being logged in on your trainer account.");
            }

            var certificates = await _dbContext.TrenerCertifikats
                .Where(tc => tc.PracownikId == trainerId)
                .Join(_dbContext.Certyfikats, tc => tc.CertyfikatId, c => c.CertyfikatId, (tc, c) => new YourCertificatesDto
                {
                    CertificateName = c.Nazwa,
                    ReceivedDate = tc.DataOtrzymania
                })
                .ToListAsync(cancellationToken);

            return certificates;
        }
    }
}
