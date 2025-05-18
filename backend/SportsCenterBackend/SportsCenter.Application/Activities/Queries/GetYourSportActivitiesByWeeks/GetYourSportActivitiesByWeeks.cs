using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetYourSportActivitiesByWeeks
{
    public class GetYourSportActivitiesByWeeks : IQuery<IEnumerable<YourSportActivityByWeeksDto>>
    {
        public int WeekOffset { get; set; } = 0;

        public GetYourSportActivitiesByWeeks(int weekOffSet)
        {
            WeekOffset = weekOffSet;
        }
    }
}
