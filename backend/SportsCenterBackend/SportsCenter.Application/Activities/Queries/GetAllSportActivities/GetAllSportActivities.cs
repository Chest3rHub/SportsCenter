using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Queries.GetAllSportActivities
{

    public class GetAllSportActivities : IQuery<IEnumerable<SportActivityDto>>
    {
        public int Offset { get; set; } = 0;

        public GetAllSportActivities(int offSet)
        {
            Offset = offSet;
        }
    }
}
