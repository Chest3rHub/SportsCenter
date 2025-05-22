using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetEmployeesPositions
{
    public class GetEmployeesPositions : IQuery<IEnumerable<EmployeePositionDto>>
    {    
        public GetEmployeesPositions()
        {
        }
    }
}
