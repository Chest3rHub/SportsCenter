using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.PayForReservation
{
    public sealed record PayForReservation : ICommand<Unit>
    {
        public int ReservationId { get; set; }

        public PayForReservation(int reservationId)
        {
            ReservationId = reservationId;
        }
    }
}
