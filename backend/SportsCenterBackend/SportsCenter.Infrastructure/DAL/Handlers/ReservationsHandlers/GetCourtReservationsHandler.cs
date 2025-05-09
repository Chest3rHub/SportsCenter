using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Reservations.Queries.GetCourtReservations;
using SportsCenter.Infrastructure.DAL;

namespace SportsCenter.Infrastructure.DAL.Handlers.ReservationsHandlers
{
    internal class GetCourtReservationsHandler : IRequestHandler<GetCourtReservations, IEnumerable<CourtReservationsDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetCourtReservationsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CourtReservationsDto>> Handle(GetCourtReservations request, CancellationToken cancellationToken)
        {
        var startOfDay = request.Date.Date;
        
        return await _dbContext.Rezerwacjas
            .Where(r => r.KortId == request.CourtId &&
                        r.DataOd.Date == startOfDay &&
                        !(r.CzyOdwolana ?? false))
            .Select(r => new CourtReservationsDto
            {
                ReservationId = r.RezerwacjaId,
                StartTime = r.DataOd,
                EndTime = r.DataDo
            })
            .ToListAsync(cancellationToken);
        }
    }
}