using SportsCenter.Application.Abstractions;
using SportsCenter.Application.Employees.Queries.GetAbsenceRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetAbsenceRequest
{
    public class GetAbsenceRequests : IQuery<IEnumerable<AbsenceRequestDto>>
    {
        public int Offset { get; set; } = 0;
        public GetAbsenceRequests(int offSet)
        {
            Offset = offSet;
        }
    }
}
