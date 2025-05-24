using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class CannotUnsubscribeFromPastEventException : Exception
    {
        public CannotUnsubscribeFromPastEventException() : base("Cannot cancel sign up for a past event")
        {
        }
    }
}
