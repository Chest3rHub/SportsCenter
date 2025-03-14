using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class InvalidActivityDateException : Exception
    {
        public DateOnly Date { get; set; }
        public InvalidActivityDateException(DateOnly date) : base($"The given date: {date} is wrong")
        {
            Date = date;
        }
    }
}
