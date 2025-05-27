using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetYourSportActivitiesByWeeks
{
    public class YourSportActivityByWeeksDto
    {
        public int? InstanceOfActivityId { get; set; }
        public int? ReservationId { get; set; }
        public string SportActivityName { get; set; }
        public DateOnly DateOfActivity { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public int DurationInMinutes { get; set; }
        public TimeSpan EndTime { get; set; }
        public string LevelName { get; set; }
        public int? EmployeeId { get; set; }
        public string? TrainerName { get; set; }
        public string CourtName { get; set; }
        public decimal? CostWithoutEquipment { get; set; }
        public decimal? CostWithEquipment { get; set; }
        public decimal reservationCost { get; set; }
        public string IsEquipmentReserved { get; set; }
        public string IsActivityPaid { get; set; }
        public string IsActivityCanceled { get; set; }
        public string Type { get; set; }
    }
}
