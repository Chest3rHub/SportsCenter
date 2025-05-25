using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Schedule.Queries.GetScheduleInfo
{
    public class ParticipantDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsPaid { get; set; }
    }
}
