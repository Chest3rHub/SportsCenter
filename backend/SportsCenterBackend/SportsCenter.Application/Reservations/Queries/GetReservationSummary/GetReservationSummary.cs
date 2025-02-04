using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Reservations.Queries.GetReservationSummary
{
    public class GetReservationSummary : IQuery<ReservationSummaryDto>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GetReservationSummary(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
