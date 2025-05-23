using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class LimitOfPlacesReachedException : Exception
    {
        public LimitOfPlacesReachedException() : base($"Limit of places reached.")
        {
        }
    }
}
