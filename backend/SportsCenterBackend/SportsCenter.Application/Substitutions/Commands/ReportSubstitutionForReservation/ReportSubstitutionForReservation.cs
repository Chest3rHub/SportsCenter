using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Substitutions.Commands.ReportSubstitutionForReservation
{
    public sealed record ReportSubstitutionForReservation : ICommand<Unit>
    {
        public int ReservationId { get; set; }
    }
}
