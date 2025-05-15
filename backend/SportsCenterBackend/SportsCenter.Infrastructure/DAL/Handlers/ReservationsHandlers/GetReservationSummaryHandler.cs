using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Reservations.Queries.GetReservationSummary;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ReservationHandlers
{
    internal class GetReservationSummaryHandler : IRequestHandler<GetReservationSummary, ReservationSummaryDto>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetReservationSummaryHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReservationSummaryDto> Handle(GetReservationSummary request, CancellationToken cancellationToken)
        {
            int pageSize = 5;
            int numberPerPage = 6;

            var startDate = request.StartDate;
            var endDate = request.EndDate;

            var summaryData = await (
                from r in _dbContext.Rezerwacjas
                join k in _dbContext.Klients on r.KlientId equals k.KlientId
                join o in _dbContext.Osobas on k.KlientId equals o.Klient.KlientId
                where r.DataOd.Date >= startDate.Date && r.DataDo.Date <= endDate.Date
                group r by new { k.KlientId, o.Email } into g
                select new ReservationGroupSummaryDto
                {
                    ClientEmail = g.Key.Email,
                    CompletedReservations = g.Count(x => x.CzyOdwolana == false),
                    CancelledReservations = g.Count(x => x.CzyOdwolana == true),
                    TotalRevenue = g
                        .Where(x => x.CzyOplacona == true && x.CzyZwroconoPieniadze == false)
                        .Sum(x => x.Koszt)
                }
            )
            .Skip(request.Offset * pageSize)
            .Take(numberPerPage)
            .ToListAsync(cancellationToken);

            return new ReservationSummaryDto
            {
                SummariesByRezerwacja = summaryData
            };
        }
    }
}
