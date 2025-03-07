using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public class ReservationOutsideWorkingHoursException : Exception
    {
        public ReservationOutsideWorkingHoursException() : base($"Reservation is outside sports center working hours.")
        {
        }
    }
}
