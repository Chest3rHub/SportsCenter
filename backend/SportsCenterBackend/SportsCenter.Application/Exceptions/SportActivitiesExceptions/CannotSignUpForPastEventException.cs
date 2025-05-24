using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class CannotSignUpForPastEventException : Exception
    {
        public CannotSignUpForPastEventException() : base("Cannot sign up for a past event")
        {
        }
    }
}
