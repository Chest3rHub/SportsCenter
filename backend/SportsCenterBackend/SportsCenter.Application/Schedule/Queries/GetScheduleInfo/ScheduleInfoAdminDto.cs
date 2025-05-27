using Newtonsoft.Json;
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
        
        [JsonProperty(Order = 13)]
        public decimal? Discount { get; set; }
        public bool? IsRecurring { get; set; }
        [JsonProperty(Order = 14)]
        public List<ParticipantDto> Participants { get; set; } = new();
        [JsonProperty(Order = 15)]
        public bool IsEquipmentReserved { get; set; }
        [JsonProperty(Order = 16)]
        public bool IsCanceled { get; set; }
        [JsonProperty(Order = 17)]
        public int ActivityIdToPay { get; set; }
    }
}
