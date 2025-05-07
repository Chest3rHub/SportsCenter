using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Queries.GetAvailableCourts
{
    public class GetAvailableCourts : IQuery<IEnumerable<CourtDto>>
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public GetAvailableCourts(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
