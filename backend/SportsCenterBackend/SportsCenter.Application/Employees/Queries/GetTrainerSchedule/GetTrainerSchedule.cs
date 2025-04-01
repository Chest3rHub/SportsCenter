using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTrainerSchedule
{
    public class GetTrainerSchedule : IQuery<IEnumerable<TrainerScheduleDto>>
    {
        public int Offset { get; set; } = 0;
        public GetTrainerSchedule(int offSet)
        {
            Offset = offSet;
        }
    }
}
