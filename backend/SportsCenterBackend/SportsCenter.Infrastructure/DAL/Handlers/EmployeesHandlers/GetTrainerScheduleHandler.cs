﻿using MediatR;
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

            var zajeciaSchedule = await _dbContext.GrafikZajecs
                .Where(dz => dz.PracownikId == trainerId)
                .ToListAsync(cancellationToken);

            var rezerwacjaSchedule = await _dbContext.Rezerwacjas
                .Where(r => r.TrenerId == trainerId)
                .ToListAsync(cancellationToken);

            var zajeciaScheduleDtos = zajeciaSchedule.Select(dz => new TrainerScheduleDto
            {
                Id = dz.ZajeciaId,
                Type = "zajecia",
                StartDate = GetStartDate(dz),
                EndDate = GetEndDate(dz),
                Name = dz.Zajecia.Nazwa,
                Level = dz.Zajecia.IdPoziomZajecNavigation.Nazwa,
                Status = _dbContext.Zastepstwos
                    .Where(z => z.ZajeciaId == dz.ZajeciaId && z.PracownikNieobecnyId == trainerId)  // Zastępstwo
                    .Select(z => z.PracownikZastepujacyId == null ? "Zastępstwo oczekuje na akceptacje" : "Zastępstwo zaakceptowane")
                    .FirstOrDefault()
            }).ToList();

            var rezerwacjaScheduleDtos = rezerwacjaSchedule.Select(r => new TrainerScheduleDto
            {
                Id = r.RezerwacjaId,
                Type = "rezerwacja",
                StartDate = r.DataOd,
                EndDate = r.DataDo,
                Name = null,
                Level = null,
                Status = _dbContext.Zastepstwos
                    .Where(z => z.RezerwacjaId == r.RezerwacjaId && z.PracownikNieobecnyId == trainerId)  // Zastępstwo w rezerwacjach
                    .Select(z => z.PracownikZastepujacyId == null ? "Zastępstwo oczekuje na akceptacje" : "Zastępstwo zaakceptowane")
                    .FirstOrDefault()
            }).ToList();

            return zajeciaScheduleDtos.Union(rezerwacjaScheduleDtos);
        }

        private DateTime GetStartDate(GrafikZajec grafik)
        {
            var today = DateTime.Today;

            var dayOfWeekMapping = new Dictionary<string, int>
            {
                { "poniedzialek", 1 },
                { "wtorek", 2 },
                { "sroda", 3 },
                { "czwartek", 4 },
                { "piatek", 5 },
                { "sobota", 6 },
                { "niedziela", 7 }
            };

            if (!dayOfWeekMapping.ContainsKey(grafik.DzienTygodnia))
            {
                throw new ArgumentException("Invalid day of the week: " + grafik.DzienTygodnia);
            }

            var targetDayOfWeek = dayOfWeekMapping[grafik.DzienTygodnia];

            var daysToAdd = (targetDayOfWeek - (int)today.DayOfWeek + 7) % 7;

            var hours = grafik.GodzinaOd.Hours;
            var minutes = grafik.GodzinaOd.Minutes;

            var startDate = today.AddDays(daysToAdd)
                                .AddHours(hours)
                                .AddMinutes(minutes);

            return startDate;
        }

        private DateTime GetEndDate(GrafikZajec grafik)
        {
            var startDate = GetStartDate(grafik);
            return startDate.AddMinutes(grafik.CzasTrwania);
        }
    }
}
