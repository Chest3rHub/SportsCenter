using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SubstitutionsExceptions
{
    public class SubstitutionForReservationNotAllowedException : Exception
    {
        public int Id { get; set; }

        public SubstitutionForReservationNotAllowedException(int id) : base($"You are not a trainer for reservation with id: {id}")
        {
            Id = id;
        }
    }
}
