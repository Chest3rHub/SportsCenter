using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Substitutions.Commands.ReportSubstitutionForActivities
{

    public sealed record ReportSubstitutionForActivities : ICommand<Unit>
    {
        public int ActivityId { get; set; }
        public DateOnly ActivityDate { get; set; }

    }
}
