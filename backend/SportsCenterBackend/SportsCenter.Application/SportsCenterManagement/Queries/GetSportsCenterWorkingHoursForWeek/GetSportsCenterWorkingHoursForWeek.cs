using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Queries.GetSportsCenterWorkingHoursForWeek
{
    public class GetSportsCenterWorkingHoursForWeek : IQuery<IEnumerable<SportsCenterWorkingHoursDto>>
    {
        public int WeekOffset {  get; set; }

        public GetSportsCenterWorkingHoursForWeek(int weekOffset)
        {
           WeekOffset = weekOffset;
        }
    }
}
