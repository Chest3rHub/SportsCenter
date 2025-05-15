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
        public int Offset { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GetReservationSummary(int offset, DateTime startDate, DateTime endDate)
        {
            Offset = offset;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
