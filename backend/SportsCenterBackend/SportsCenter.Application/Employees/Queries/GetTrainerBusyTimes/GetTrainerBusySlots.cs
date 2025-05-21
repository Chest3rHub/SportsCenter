using SportsCenter.Application.Abstractions;
using SportsCenter.Application.Employees.Queries.GetTrainerSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Employees.Queries.GetTrainerBusyTimes
{
    public class GetTrainerBusySlots : IQuery<IEnumerable<BusyTimesSlotDto>>
    {
        public int TrainerId { get; set; }
        public DateTime Date { get; set; }

        public GetTrainerBusySlots(int trainerId, DateTime date) 
        { 
            TrainerId = trainerId;
            Date = date;
        }
    }
}
