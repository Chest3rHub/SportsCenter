using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsCenter.Application.Employees.Commands.DismissEmployee
{
    public sealed record DismissEmployee : ICommand<Unit>
    {
        public int EmployeeId { get; set; }
        public DismissEmployee(int employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
