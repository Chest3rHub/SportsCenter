using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public class NotThatClientReservationException : Exception
    {
        public int ClientId { get; set; }
        public int ReservationId { get; set; }

        public NotThatClientReservationException(int clientId, int reservationId) : base($"Client with id: {clientId} does not have a reservation with id: {reservationId}")
        {
            ClientId = clientId;
            ReservationId = reservationId;
        }
    }
}
