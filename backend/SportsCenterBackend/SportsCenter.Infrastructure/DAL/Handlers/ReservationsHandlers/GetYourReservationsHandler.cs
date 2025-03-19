using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Reservations.Queries.GetYourReservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ReservationsHandlers
{
    internal class GetYourReservationsHandler : IRequestHandler<GetYourReservations, IEnumerable<YourReservationDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetYourReservationsHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<YourReservationDto>> Handle(GetYourReservations request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your account.");
            }

            var reservations = await _dbContext.Rezerwacjas
                  .Where(r => r.KlientId == userId)
                  .Select(r => new YourReservationDto
                  {
                      CourtId = r.KortId,
                      StartTime = r.DataOd,
                      EndTime = r.DataDo,
                      TrainerId = r.TrenerId,
                      IsEquipmentReserved = r.CzyUwzglednicSprzet,
                      Cost = r.Koszt,
                      IsReservationPaid = r.CzyOplacona.HasValue
                        ? (r.CzyOplacona.Value ? "Tak" : "Nie")
                        : "Nie",
                      IsReservationCanceled = r.CzyOdwolana.HasValue 
                        ? (r.CzyOdwolana.Value ? "Tak" : "Nie")
                        : "Nie",
                      IsMoneyRefunded = r.CzyZwroconoPieniadze.HasValue
                        ? (r.CzyZwroconoPieniadze.Value ? "Tak" : "Nie")
                        : "Nie",
                  })
                  .ToListAsync(cancellationToken);

            return reservations;
        }
    }
}
