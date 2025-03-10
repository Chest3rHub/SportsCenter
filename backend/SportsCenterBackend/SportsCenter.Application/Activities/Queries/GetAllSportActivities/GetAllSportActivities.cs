using MediatR;
using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Activities.Queries.GetAllSportActivities
{

    public class GetAllSportActivities : IQuery<IEnumerable<SportActivityDto>>
    {
    }
}
