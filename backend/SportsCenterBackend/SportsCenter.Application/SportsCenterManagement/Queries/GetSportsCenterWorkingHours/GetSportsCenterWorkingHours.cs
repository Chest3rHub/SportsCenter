using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Queries.GetSportsCenterWorkingHours
{
    public class GetSportsCenterWorkingHours : IQuery<SportsCenterWorkingHoursDto>
    {
        public DateTime TargetDate { get; set; }

        public GetSportsCenterWorkingHours(DateTime targetDate)
        {
            TargetDate = targetDate;
        }
    }
}
