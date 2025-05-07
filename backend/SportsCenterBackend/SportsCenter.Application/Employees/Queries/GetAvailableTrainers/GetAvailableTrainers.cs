using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetAvailableTrainers
{
    public class GetAvailableTrainers : IQuery<IEnumerable<TrainerDto>>
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public GetAvailableTrainers(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
