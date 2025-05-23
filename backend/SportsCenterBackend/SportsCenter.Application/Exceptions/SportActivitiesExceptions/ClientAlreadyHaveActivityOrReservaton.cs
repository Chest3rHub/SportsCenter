using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ClientAlreadyHasActivityOrReservationException : Exception
    {
        public ClientAlreadyHasActivityOrReservationException(DateOnly conflictTime)
            : base($"Client already has another activity or reservation around {conflictTime}.") { }
    }

}
