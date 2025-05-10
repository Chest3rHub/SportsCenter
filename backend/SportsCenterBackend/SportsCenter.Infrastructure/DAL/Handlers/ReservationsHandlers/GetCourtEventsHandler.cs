using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Reservations.Queries.getCourtEvents;
using SportsCenter.Infrastructure.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ReservationsHandlers
{
    internal class getCourtEventsHandler : IRequestHandler<getCourtEvents, IEnumerable<CourtEventsDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly Dictionary<DayOfWeek, string> _dniTygodnia = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Monday, "poniedzialek" },
            { DayOfWeek.Tuesday, "wtorek" },
            { DayOfWeek.Wednesday, "sroda" },
            { DayOfWeek.Thursday, "czwartek" },
            { DayOfWeek.Friday, "piatek" },
            { DayOfWeek.Saturday, "sobota" },
            { DayOfWeek.Sunday, "niedziela" }
        };

        public getCourtEventsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CourtEventsDto>> Handle(getCourtEvents request, CancellationToken cancellationToken)
        {
            var startOfDay = request.Date.Date;
            var dayOfWeek = _dniTygodnia[request.Date.DayOfWeek];
            var reservations = await _dbContext.Rezerwacjas
                .Where(r => r.KortId == request.CourtId &&
                           r.DataOd.Date == startOfDay &&
                           !(r.CzyOdwolana ?? false))
                .Select(r => new CourtEventsDto
                {
                    EventId = r.RezerwacjaId,
                    StartTime = r.DataOd,
                    EndTime = r.DataDo,
                    IsReservation = true
                })
                .ToListAsync(cancellationToken);

            
            var activities = await _dbContext.GrafikZajecs
                .Where(g => g.KortId == request.CourtId &&
                           g.DzienTygodnia == dayOfWeek)
                .Select(g => new CourtEventsDto
                {
                    EventId = g.GrafikZajecId,
                    StartTime = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day)
                        .Add(g.GodzinaOd),
                    EndTime = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day)
                        .Add(g.GodzinaOd)
                        .AddMinutes(g.CzasTrwania),
                    IsReservation = false
                })
                .ToListAsync(cancellationToken);

            return reservations.Concat(activities)
                .OrderBy(x => x.StartTime)
                .ToList();
        }
    }
}