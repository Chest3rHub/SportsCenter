using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Schedule.Queries.GetScheduleInfo
{
    public class ScheduleInfoDto
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int CourtNumber { get; set; }
        public string? TrainerName { get; set; }
  
        public List<string> Participants { get; set; } = new List<string>();
        public decimal? ReservationCost { get; set; } 
        public decimal? Discount { get; set; }
        public bool? IsRecurring { get; set; }
      
        public string? GroupName { get; set; }
        public string? SkillLevel { get; set; }
    }
}
