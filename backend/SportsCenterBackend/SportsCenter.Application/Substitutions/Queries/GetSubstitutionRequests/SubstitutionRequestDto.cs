using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportsCenter.Application.Substitutions.Queries.GetSubstitutionRequestsForActivities
{
    public class SubstitutionRequestDto
    {
        public int SubstitutionId { get; set; }
        public DateOnly Date { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ActivityId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ReservationId { get; set; }
        public int EmployeeId { get; set; }
    }
}
