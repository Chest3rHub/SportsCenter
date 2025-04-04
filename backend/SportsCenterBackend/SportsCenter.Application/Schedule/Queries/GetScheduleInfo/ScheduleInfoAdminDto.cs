﻿using Newtonsoft.Json;
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
        [JsonProperty(Order = 10)]
        public decimal? Cost { get; set; }
        [JsonProperty(Order = 11)]
        public decimal? Discount { get; set; }
        public bool? IsRecurring { get; set; }
        [JsonProperty(Order = 12)]
        public List<string> Participants { get; set; } = new List<string>();
    }
}
