using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Schedule.Queries.GetScheduleInfo
{
    public class ScheduleInfoTrainerDto : ScheduleInfoBaseDto
    {
        public ScheduleInfoTrainerDto() => Type = "Trainer";
        [JsonProperty(Order = 13)]
        public List<ParticipantDto> Participants { get; set; } = new();

    }
}
