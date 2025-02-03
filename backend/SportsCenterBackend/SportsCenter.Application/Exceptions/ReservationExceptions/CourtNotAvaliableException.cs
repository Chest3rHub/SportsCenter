using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public sealed class CourtNotAvaliableException : Exception
    {
        public int Id { get; set; }

        public CourtNotAvaliableException(int id) : base($"Court with id: {id} is occupied at this time")
        {
            Id = id;
        }
    }
}
