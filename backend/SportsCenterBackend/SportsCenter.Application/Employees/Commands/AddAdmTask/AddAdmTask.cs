using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AddAdmTask
{
    public sealed record AddAdmTask : ICommand<Unit>
    {
        public string Description { get; set; } = null!;
        public DateTime DateTo { get; set; }
        public int EmployeeId { get; set; }
        
        public AddAdmTask(string description, DateTime dateTo, int employeeId)
        {
            Description = description;
            DateTo = dateTo;
            EmployeeId = employeeId;
        }
    }
}
