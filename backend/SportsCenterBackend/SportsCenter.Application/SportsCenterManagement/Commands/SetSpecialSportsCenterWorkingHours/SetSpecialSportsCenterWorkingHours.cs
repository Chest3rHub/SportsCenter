using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsCenterManagement.Commands.SetSpecialSportsCenterWorkingHours
{
    public sealed record SetSpecialSportsCenterWorkingHours : ICommand<Unit>
    {
        public DateTime Date { get; set; }
        public TimeSpan OpenHour { get; set; }
        public TimeSpan CloseHour { get; set; }     

        public SetSpecialSportsCenterWorkingHours(DateTime date, TimeSpan openHour, TimeSpan closeHour)
        {
            Date = date;
            OpenHour = openHour;
            CloseHour = closeHour;
        }
    }
}
