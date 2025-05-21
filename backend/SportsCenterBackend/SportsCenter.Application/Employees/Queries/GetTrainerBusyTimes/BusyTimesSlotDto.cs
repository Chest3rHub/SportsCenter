using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTrainerBusyTimes
{
    public class BusyTimesSlotDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
