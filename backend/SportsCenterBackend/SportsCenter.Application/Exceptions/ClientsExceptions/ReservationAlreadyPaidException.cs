using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ClientsExceptions
{
    public class ReservationAlreadyPaidException : Exception
    {
        public int Id;
        public ReservationAlreadyPaidException(int id) : base($"Reservation with id: {id} already paid")
        {
            Id = id;
        }
    }
}
