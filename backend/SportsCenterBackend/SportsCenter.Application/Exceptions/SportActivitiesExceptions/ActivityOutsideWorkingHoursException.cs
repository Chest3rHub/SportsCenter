using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ActivityOutsideWorkingHoursException : Exception
    {
        public ActivityOutsideWorkingHoursException() : base($"Activity is outside sports center working hours.")
        {
        }
    }
}
