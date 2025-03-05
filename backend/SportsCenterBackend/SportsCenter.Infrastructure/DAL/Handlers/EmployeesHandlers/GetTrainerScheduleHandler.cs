using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetTrainerSchedule;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetTrainerScheduleHandler : IRequestHandler<GetTrainerSchedule, IEnumerable<TrainerScheduleDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetTrainerScheduleHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<TrainerScheduleDto>> Handle(GetTrainerSchedule request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int trainerId))
            {
                throw new UnauthorizedAccessException("You cannot access your schedule without being logged in on your trainer account.");
            }

            var zajeciaSchedule = await _dbContext.DataZajecs
                .Where(dz => dz.GrafikZajec.PracownikId == trainerId)
                .Select(dz => new TrainerScheduleDto
                {
                    Id = dz.GrafikZajec.ZajeciaId,
                    Type = "zajecia",
                    StartDate = dz.Date,
                    EndDate = dz.Date.AddMinutes(dz.GrafikZajec.CzasTrwania),
                    Name = dz.GrafikZajec.Zajecia.Nazwa,
                    Level = dz.GrafikZajec.Zajecia.IdPoziomZajecNavigation.Nazwa,
                    Status = _dbContext.Zastepstwos
                        .Where(z => z.ZajeciaId == dz.GrafikZajec.ZajeciaId && z.PracownikNieobecnyId == trainerId)
                        .Select(z => z.PracownikZastepujacyId == null ? "Zastępstwo oczekuje na akceptacje" : "Zastępstwo zaakceptowane")
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            var rezerwacjaSchedule = await _dbContext.Rezerwacjas
                .Where(r => r.TrenerId == trainerId)
                .Select(r => new TrainerScheduleDto
                {
                    Id = r.RezerwacjaId,
                    Type = "rezerwacja",
                    StartDate = r.DataOd,
                    EndDate = r.DataDo,
                    Name = null,
                    Level = null,
                    Status = _dbContext.Zastepstwos
                        .Where(z => z.RezerwacjaId == r.RezerwacjaId && z.PracownikNieobecnyId == trainerId)
                        .Select(z => z.PracownikZastepujacyId == null ? "Zastępstwo oczekuje na akceptacje" : "Zastępstwo zaakceptowane")
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            return zajeciaSchedule.Union(rezerwacjaSchedule);
        }
    }
}
