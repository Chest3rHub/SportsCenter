using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Queries.GetAllSportActivities
{

    public class GetAllSportActivities : IQuery<IEnumerable<SportActivityDto>>
    {
        public int WeekOffset { get; set; } = 0;

        public GetAllSportActivities(int weekOffSet)
        {
            WeekOffset = weekOffSet;
        }
    }
}
