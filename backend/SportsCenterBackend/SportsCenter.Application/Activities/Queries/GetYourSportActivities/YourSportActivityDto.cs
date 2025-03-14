using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetYourSportActivities
{
    public class YourSportActivityDto
    {
        public int InstanceOfActivityId { get; set; }
        public string SportActivityName { get; set; }
        public DateOnly DateOfActivity { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartHour { get; set; }
        public int DurationInMinutes { get; set; }
        public string LevelName { get; set; }
        public int EmployeeId { get; set; }
        public string CourtName { get; set; }
        public decimal CostWithoutEquipment { get; set; }
        public decimal CostWithEquipment { get; set; }
        public string IsActivityPaid { get; set; }
        public string IsActivityCanceled { get; set; }
    }
}
