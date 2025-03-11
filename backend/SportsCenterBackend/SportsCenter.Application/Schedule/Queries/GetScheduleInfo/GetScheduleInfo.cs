using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;

namespace SportsCenter.Application.Schedule.Queries.GetScheduleInfo
{
    public class GetScheduleInfo : IQuery<List<ScheduleInfoBaseDto>>
    {
        public int WeekOffset { get; set; } = 0;

        public GetScheduleInfo(int weekOffSet)
        {
            WeekOffset = weekOffSet;
        }
    }
}
