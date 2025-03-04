using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.SportsClubManagement.Commands.AddSportsClubWorkingHours
{
    public sealed record SetSportsCenterWorkingHours : ICommand<Unit>
    {
         
        public TimeSpan OpenHour { get; set; }
        public TimeSpan CloseHour { get; set; }
        public string DayOfWeek { get; set; }

        public SetSportsCenterWorkingHours( TimeSpan openHour, TimeSpan closeHour, string dayOfWeek )
        {
            OpenHour = openHour;
            CloseHour = closeHour;
            DayOfWeek = dayOfWeek;
        }

    }
}
