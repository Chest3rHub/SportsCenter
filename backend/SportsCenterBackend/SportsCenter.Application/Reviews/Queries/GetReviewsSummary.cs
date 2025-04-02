using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reviews.Queries
{
    public class GetReviewsSummary : IQuery<IEnumerable<ReviewsSummaryDto>>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Offset { get; set; } = 0;

        public GetReviewsSummary(DateTime startDate, DateTime endDate, int offset)
        {
            StartDate = startDate;
            EndDate = endDate;
            Offset = offset;
        }
    }
}
