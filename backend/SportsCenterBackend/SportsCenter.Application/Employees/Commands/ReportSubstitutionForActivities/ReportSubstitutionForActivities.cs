using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.ReportSubstitutionNeed
{

    public sealed record ReportSubstitutionForActivities : ICommand<Unit>
    {
        public int ActivitiesId { get; set; }

    }
}
