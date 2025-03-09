using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public class PostponeReservationNotAllowedException : Exception
    {
        public PostponeReservationNotAllowedException() : base($"Client can only postpone the reservation up to 24 hours before its start date.")
        {
        }
    }
}
