using SportsCenter.Application.Abstractions;
using SportsCenter.Application.Employees.Queries.GetAbsenceRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetYourAbsenceRequests
{
    public class GetYourAbsenceRequests : IQuery<IEnumerable<YourAbsenceRequestsDto>>
    {
        public int Offset { get; set; } = 0;
        public GetYourAbsenceRequests(int offSet)
        {
            Offset = offSet;
        }
    }
}
