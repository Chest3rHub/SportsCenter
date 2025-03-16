using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ActivityAlreadyCanceledException : Exception
    {

        public ActivityAlreadyCanceledException() : base($"Acivity instance is already canceled")
        {
        }
    }
}
