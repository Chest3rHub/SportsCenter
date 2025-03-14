using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetSportActivity
{
    public class SportActivityDto
    {
        public int SportActivityId { get; set; }
        public DateOnly StartDate { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly StartHour { get; set; }
        public int DurationInMinutes { get; set; }
        public string ActivityName { get; set; }
        public string LevelName { get; set; }
        public string CourtName { get; set; }
        public int MaxParticipants { get; set; }
        public decimal CostWithoutEquipment { get; set; }
        public decimal CostWithEquipment { get; set; }
    }
}
