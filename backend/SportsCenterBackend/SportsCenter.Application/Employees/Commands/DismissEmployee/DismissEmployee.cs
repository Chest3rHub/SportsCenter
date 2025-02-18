using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsCenter.Application.Employees.Commands.DismissEmployee
{
    public sealed record DismissEmployee : ICommand<ReservationFailureResponse>
    {
        public int DismissedEmployeeId { get; set; }
        public DateTime DismissalDate { get; set; }
        public DismissEmployee(int dismissedEmployeeId, DateTime dismissalDate)
        {
            DismissedEmployeeId = dismissedEmployeeId;
            DismissalDate = dismissalDate;
        }
    }
}
