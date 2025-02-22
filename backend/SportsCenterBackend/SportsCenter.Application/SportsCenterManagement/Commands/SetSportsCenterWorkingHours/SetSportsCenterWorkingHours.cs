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
        public int DayOfWeekId { get; set; }   
        public TimeSpan OpenHour { get; set; }
        public TimeSpan CloseHour { get; set; }

        public SetSportsCenterWorkingHours(int dayOfWeek, TimeSpan openHour, TimeSpan closeHour)
        {
            DayOfWeekId = dayOfWeek;
            OpenHour = openHour;
            CloseHour = closeHour;
        }

    }
}
