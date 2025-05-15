using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Reservations.Queries.GetReservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ReservationsHandlers
{
    internal sealed class GetReservationHandler : IRequestHandler<GetReservation, ReservationDto>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetReservationHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReservationDto> Handle(GetReservation request, CancellationToken cancellationToken)
        {
            var reservation = await _dbContext.Rezerwacjas
                .Where(r => r.RezerwacjaId == request.ReservationId)
                .Select(r => new ReservationDto
                {
                    ReservationId = r.RezerwacjaId,
                    ClientEmail = r.Klient.KlientNavigation.Email,
                    CourtName = r.Kort.Nazwa,
                    StartTime = r.DataOd,
                    EndTime = r.DataDo,
                    TrainerId = r.TrenerId,
                    Trainer = r.Trener.PracownikNavigation.Imie + " " + r.Trener.PracownikNavigation.Nazwisko,
                    IsEquipmentReserved = r.CzyUwzglednicSprzet,
                    Cost = r.Koszt,
                    IsReservationPaid = r.CzyOplacona ?? false,
                    IsReservationCanceled = r.CzyOdwolana ?? false,
                    IsMoneyRefunded = r.CzyZwroconoPieniadze ?? false
                })
                .FirstOrDefaultAsync(cancellationToken);

            return reservation;
        }
    }
}
