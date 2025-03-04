using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Queries.GetSportsCenterWorkingHours
{
    public class GetSportsCenterWorkingHours : IQuery<IEnumerable<SportsCenterWorkingHoursDto>>
    {
        public DateOnly StartDate {  get; set; }

        public GetSportsCenterWorkingHours(DateOnly startDate)
        {
            StartDate = startDate;
        }
    }
}
