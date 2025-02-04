using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Reservations.Queries.GetReservationSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ReservationsHandlers
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
            int reservationCount = await _dbContext.Rezerwacjas
                //dodalam 1 dzien do end date by zrobic ze mozna utworzyc rezerwacje do 23:59 w dacie end date i sie to zliczy do ilosci rezerwacji
                .CountAsync(r => r.DataOd < request.EndDate.AddDays(1) && r.DataDo >= request.StartDate, cancellationToken);

            return new ReservationSummaryDto{TotalReservations = reservationCount };
        }
    }
}
