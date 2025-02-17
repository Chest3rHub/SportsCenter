using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;

namespace SportsCenter.Application.Schedule.Queries.GetScheduleInfo
{
    public class GetScheduleInfo : IQuery<List<ScheduleInfoBaseDto>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
      
        public GetScheduleInfo(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
