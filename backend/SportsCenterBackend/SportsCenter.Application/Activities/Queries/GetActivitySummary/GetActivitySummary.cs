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
        public int Offset { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public GetActivitySummary(int offset, DateTime startDate, DateTime endDate)
        {
            Offset = offset;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
