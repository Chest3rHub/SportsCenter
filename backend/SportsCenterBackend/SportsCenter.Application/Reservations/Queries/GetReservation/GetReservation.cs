using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Reservations.Queries.GetReservation
{
    public class GetReservation : IQuery<ReservationDto>
    {
        public int ReservationId { get; set; }

        public GetReservation(int reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
