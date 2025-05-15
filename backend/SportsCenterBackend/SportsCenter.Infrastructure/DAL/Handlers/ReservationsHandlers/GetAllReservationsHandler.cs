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
    internal class GetAllReservationsHandler : IRequestHandler<GetAllReservations, IEnumerable<AllReservationsDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllReservationsHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<AllReservationsDto>> Handle(GetAllReservations request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access your absence requests without being logged in on your account.");
            }

            int pageSize = 6;
            int numberPerPage = 7;

            var reservations = await _dbContext.Rezerwacjas
                  .OrderByDescending(r => r.DataOd)
                  .Skip(request.Offset * pageSize)
                  .Take(numberPerPage)
                  .Select(r => new AllReservationsDto
                  {
                      ReservationId = r.RezerwacjaId,
                      ClientEmail = r.Klient.KlientNavigation.Email,
                      CourtName = r.Kort.Nazwa,
                      StartTime = r.DataOd,
                      EndTime = r.DataDo,
                      Trainer = r.Trener.PracownikNavigation.Imie + " " + r.Trener.PracownikNavigation.Nazwisko,
                      IsEquipmentReserved = r.CzyUwzglednicSprzet,
                      Cost = r.Koszt,
                      IsReservationPaid = r.CzyOplacona ?? false,
                      IsReservationCanceled = r.CzyOdwolana ?? false,
                      IsMoneyRefunded = r.CzyZwroconoPieniadze ?? false
                  })
                  .ToListAsync(cancellationToken);

            return reservations;
        }
    }
}