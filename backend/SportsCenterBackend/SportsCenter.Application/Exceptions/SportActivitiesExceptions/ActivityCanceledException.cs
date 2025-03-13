using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ActivityCanceledException : Exception
    {
        public int Id { get; set; }

        public ActivityCanceledException(int id) : base($"SportActivity with id: {id} is canceled")
        {
            Id = id;
        }
    }
}
