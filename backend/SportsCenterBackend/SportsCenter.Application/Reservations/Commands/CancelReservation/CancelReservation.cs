using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.CancelReservation
{
    public sealed record CancelReservation : ICommand<Unit>
    {
        public int ReservationId { get; set; }

        public CancelReservation(int reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
