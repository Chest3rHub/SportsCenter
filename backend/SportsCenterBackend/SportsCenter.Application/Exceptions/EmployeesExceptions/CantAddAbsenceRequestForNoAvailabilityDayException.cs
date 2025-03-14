using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class CantAddAbsenceRequestForNoAvailabilityDayException : Exception
    {
        public CantAddAbsenceRequestForNoAvailabilityDayException() : base("This day is already marked as your unavailability day")
        {
        }
    }
}
