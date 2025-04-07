using MediatR;
using SportsCenter.Application.Activities.Queries.GetYourSportActivities;
using System.Collections.Generic;

namespace SportsCenter.Application.Activities.Queries.GetYourUpcomingActivities
{
    public class GetYourUpcomingActivities : IRequest<IEnumerable<YourSportActivityDto>>
    {
        public int Limit { get; set; } = 5; //do ustalenia

        public GetYourUpcomingActivities(int limit)
        {
            Limit = limit;
        }
    }
}