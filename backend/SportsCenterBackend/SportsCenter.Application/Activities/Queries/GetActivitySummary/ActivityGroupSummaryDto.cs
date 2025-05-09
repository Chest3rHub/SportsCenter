using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetActivitySummary
{
    public class ActivityGroupSummaryDto
    {
        public string ZajeciaNazwa { get; set; } = string.Empty;
        public int CompletedActivities { get; set; }
        public int CancelledActivities { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
