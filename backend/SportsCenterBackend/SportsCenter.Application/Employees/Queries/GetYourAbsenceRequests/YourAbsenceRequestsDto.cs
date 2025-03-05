using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetYourAbsenceRequests
{
    public class YourAbsenceRequestsDto
    {
        public int RequestId { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public bool isApproved { get; set; }
    }
}
