using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Activities.Queries.GetYourSportActivities
{
    public class GetYourSportActivities : IQuery<IEnumerable<YourSportActivityDto>>
    {
        public int Offset { get; set; } = 0;

        public GetYourSportActivities(int offSet)
        {
            Offset = offSet;
        }
    }
}
