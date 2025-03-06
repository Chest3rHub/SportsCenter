using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Substitutions.Commands.AssignSubstitution
{
    public sealed record AssignSubstitution : ICommand<Unit>
    {
        public int SubstitutionId { get; set; }
        public int SubstituteEmployeeId { get; set; }
    }
}
