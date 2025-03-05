using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public class DuplicateSubstitutionReservationRequestException : Exception
    {
        public int Id { get; set; }
        public DuplicateSubstitutionReservationRequestException(int id) : base($"Substitution request for reservation with id: {id} has already been submitted.")
        {
            Id = id;
        }
    }
}
