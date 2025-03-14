using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetActivitySummary
{
    public class GetActivitySummary : IQuery<ActivitySummaryDto>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public GetActivitySummary(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
