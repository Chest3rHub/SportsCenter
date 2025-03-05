using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.AcceptAbsenceRequest
{
    public sealed record AcceptAbsenceRequest : ICommand<Unit>
    {
        public int RequestId { get; set; }
    }
}
