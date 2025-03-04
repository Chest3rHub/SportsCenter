using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class CantAddAbsenceRequestException : Exception
    {    
        public CantAddAbsenceRequestException() : base("You have activities or reservations that time. Please report the need for a replacement.")
        {    
        }
    }
}
