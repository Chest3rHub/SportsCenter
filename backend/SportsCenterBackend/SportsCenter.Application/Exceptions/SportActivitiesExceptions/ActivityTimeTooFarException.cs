using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ActivityTimeTooFarException : Exception
    {
        public ActivityTimeTooFarException() : base($"The activity is more than 48 hours away from now.")
        {
        }
    }
}
