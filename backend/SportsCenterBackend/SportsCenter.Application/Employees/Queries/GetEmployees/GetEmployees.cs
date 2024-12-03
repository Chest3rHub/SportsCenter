using SportsCenter.Application.Abstractions;
using SportsCenter.Application.Employees.Queries.GetEmployees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetEmployees
{
    public class GetEmployees : IQuery<IEnumerable<EmployeeDto>>
    {
    }
}
