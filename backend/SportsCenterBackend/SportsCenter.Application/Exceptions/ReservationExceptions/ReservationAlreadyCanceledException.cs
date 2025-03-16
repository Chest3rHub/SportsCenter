using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public class ReservationAlreadyCanceledException : Exception
    {
        public int Id { get; set; }

        public ReservationAlreadyCanceledException(int id) : base($"Reservation with id: {id} is already canceled.")
        {
            Id = id;
        }
    }
}
