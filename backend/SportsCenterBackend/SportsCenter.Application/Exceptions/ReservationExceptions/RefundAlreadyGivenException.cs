using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public class RefundAlreadyGivenException : Exception
    {
        public int ClientId { get; set; }
        public int ReservationId { get; set; }

        public RefundAlreadyGivenException(int clientId, int reservationId) : base($"Refund for reservation with id: {reservationId} already given to te client with id: {clientId}.")
        {
            ClientId = clientId;
            ReservationId = reservationId;
        }
    }
}
