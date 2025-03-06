using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Enums
{
    public class TrainerAvailiabilityStatus
    {
        public enum TrainerAvailabilityStatus
        {
            Available,   
            HasReservations,
            HasActivities,
            IsUnavailable,
            IsFired
        }
    }
}
