using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportsCenter.Application.Substitutions.Queries.GetSubstitutionRequestsForActivities
{
    public class GetSubstitutionRequests : IQuery<IEnumerable<SubstitutionRequestDto>>
    {
        public int Offset { get; set; } = 0;
        public GetSubstitutionRequests(int offSet)
        {
            Offset = offSet;
        }
    }
}
