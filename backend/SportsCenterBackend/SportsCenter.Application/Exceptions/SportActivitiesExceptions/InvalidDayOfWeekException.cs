using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class InvalidDayOfWeekException : Exception
    {
        public string DayOfWeek { get; set; }
        public DateOnly Date {  get; set; }
        public InvalidDayOfWeekException(string dayOfWeek, DateOnly date) : base($"The given day: {dayOfWeek} does not match to date: {date}")
        {
            DayOfWeek = dayOfWeek;
            Date = date;
        }      
    }
}
