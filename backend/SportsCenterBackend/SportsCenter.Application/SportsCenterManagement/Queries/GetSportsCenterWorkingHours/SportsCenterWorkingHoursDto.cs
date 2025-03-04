using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Queries.GetSportsCenterWorkingHours
{
    public class SportsCenterWorkingHoursDto
    {
        public DateOnly Date {  get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan OpenHour { get; set; }
        public TimeSpan CloseHour { get; set; }

    }
}
