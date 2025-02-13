using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Commands.DismissEmployee
{
    public class ReservationFailureResponse
    {
        public List<int> FailedReservationIds { get; set; }

        public ReservationFailureResponse(List<int> failedReservationIds)
        {
            FailedReservationIds = failedReservationIds;
        }
    }
}
