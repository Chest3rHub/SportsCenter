using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Schedule.Queries.GetScheduleInfo
{
    public class ScheduleInfoAdminDto : ScheduleInfoBaseDto
    {
        public ScheduleInfoAdminDto() => Type = "Admin";
        public List<string> Participants { get; set; } = new List<string>();
        public decimal? ReservationCost { get; set; } 
        public decimal? Discount { get; set; }
        public bool? IsRecurring { get; set; }  
    }
}
