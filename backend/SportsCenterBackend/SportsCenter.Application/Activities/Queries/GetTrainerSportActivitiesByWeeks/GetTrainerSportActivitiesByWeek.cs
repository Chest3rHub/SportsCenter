using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetTrainerSportActivitiesByWeeks
{
    public class GetTrainerSportActivitiesByWeek : IQuery<IEnumerable<TrainerSportActivityByWeeksDto>>
    {
        public int WeekOffset { get; set; } = 0;

        public GetTrainerSportActivitiesByWeek(int weekOffSet)
        {
            WeekOffset = weekOffSet;
        }
    }
}
