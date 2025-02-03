using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ReservationExceptions
{
    public sealed class TooManyParticipantsException : Exception
    {

        public TooManyParticipantsException() : base($"There can be maximum 8 participants")
        {
        
        }

    }
}
