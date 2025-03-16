using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public class TooLateToCancelreservationException : Exception
    {
        public int Id { get; set; }

        public TooLateToCancelreservationException(int id) : base($"It is too late to cancel reservation with id: {id}")
        {
            Id = id;
        }
    }
}
