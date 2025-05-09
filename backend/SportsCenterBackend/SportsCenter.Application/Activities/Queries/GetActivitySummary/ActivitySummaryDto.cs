using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetActivitySummary
{
    public class ActivitySummaryDto
    {
        public List<ActivityGroupSummaryDto> SummariesByZajecia { get; set; } = new();
    }
}
