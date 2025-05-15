using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Queries.GetReservationSummary
{
    public class ReservationGroupSummaryDto
    {
        public string ClientEmail { get; set; } = string.Empty;
        public int CompletedReservations { get; set; }
        public int CancelledReservations { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
