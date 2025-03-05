using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetAbsenceRequests
{
    public class AbsenceRequestDto
    {
        public int RequestId { get; set; }
        public DateOnly Date {  get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
        public int EmployeeId { get; set; }

    }
}
