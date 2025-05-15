using SportsCenter.Application.Activities.Queries.GetActivitySummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Queries.GetReservationSummary
{
    public class ReservationSummaryDto
    {
        public List<ReservationGroupSummaryDto> SummariesByRezerwacja { get; set; } = new();
    }
}
