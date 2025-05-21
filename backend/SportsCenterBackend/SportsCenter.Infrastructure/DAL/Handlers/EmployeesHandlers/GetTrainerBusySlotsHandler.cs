using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Employees.Queries.GetTrainerBusyTimes;
using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.EmployeesHandlers
{
    internal class GetTrainerBusySlotsHandler : IRequestHandler<GetTrainerBusySlots, IEnumerable<BusyTimesSlotDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetTrainerBusySlotsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BusyTimesSlotDto>> Handle(GetTrainerBusySlots request, CancellationToken cancellationToken)
        {
            var busySlots = new List<BusyTimesSlotDto>();

            var dayStart = request.Date.Date;
            var dayEnd = dayStart.AddDays(1);

            // rezerwacje
            var reservations = await _dbContext.Rezerwacjas
                .Where(r =>
                    r.TrenerId == request.TrainerId &&
                    r.CzyOdwolana == false &&
                    r.DataOd >= dayStart && r.DataOd < dayEnd)
                .Select(r => new BusyTimesSlotDto
                {
                    StartTime = r.DataOd,
                    EndTime = r.DataDo
                })
                .ToListAsync(cancellationToken);

            busySlots.AddRange(reservations);

            //grafik zajęć
            var dayOfWeek = request.Date.DayOfWeek switch
            {
                DayOfWeek.Monday => "poniedzialek",
                DayOfWeek.Tuesday => "wtorek",
                DayOfWeek.Wednesday => "sroda",
                DayOfWeek.Thursday => "czwartek",
                DayOfWeek.Friday => "piatek",
                DayOfWeek.Saturday => "sobota",
                DayOfWeek.Sunday => "niedziela",
                _ => ""
            };

            var zajecia = await _dbContext.GrafikZajecs
                .Where(g => g.PracownikId == request.TrainerId && g.DzienTygodnia == dayOfWeek)
                .ToListAsync(cancellationToken);

            foreach (var z in zajecia)
            {
                var start = dayStart.Add(z.GodzinaOd);
                var end = start.AddMinutes(z.CzasTrwania);
                busySlots.Add(new BusyTimesSlotDto
                {
                    StartTime = start,
                    EndTime = end
                });
            }

            //braki dostępności
            var nieobecnosci = await _dbContext.BrakDostepnoscis
                .Where(b => b.PracownikId == request.TrainerId && b.Data == DateOnly.FromDateTime(request.Date))
                .ToListAsync(cancellationToken);

            foreach (var n in nieobecnosci)
            {
                busySlots.Add(new BusyTimesSlotDto
                {
                    StartTime = n.Data.ToDateTime(n.GodzinaOd),
                    EndTime = n.Data.ToDateTime(n.GodzinaDo)
                });
            }

            return busySlots;
        }
    }
}
