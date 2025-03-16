using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Commands.CancelClientReservation
{
    public sealed record CancelClientReservation : ICommand<Unit>
    {
        public string ClientEmail {  get; set; }
        public int ReservationId { get; set; }

        public CancelClientReservation(int reservationId, string clientEmail)
        {
            ReservationId = reservationId;
            ClientEmail = clientEmail;
        }
    }
}
