using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.PayForClientReservation
{
    public sealed record PayForClientReservation : ICommand<Unit>
    {
        public int ReservationId { get; set; }
        public string ClientEmail { get; set; }

        public PayForClientReservation(int reservationId, string clientEmail)
        {
            ReservationId = reservationId;
            ClientEmail = clientEmail;
        }
    }
}
