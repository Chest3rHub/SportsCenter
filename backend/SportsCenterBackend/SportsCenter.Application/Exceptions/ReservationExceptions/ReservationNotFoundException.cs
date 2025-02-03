using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public sealed class ReservationNotFoundException : Exception
    {
        public int Id { get; set; }

        public ReservationNotFoundException(int id) : base($"Reservation with id: {id} not found")
        {
            Id = id;
        }
    }
}
