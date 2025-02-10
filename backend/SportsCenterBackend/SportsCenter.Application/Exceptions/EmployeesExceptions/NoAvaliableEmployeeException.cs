using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class NoAvaliableEmployeeException : Exception
    {
        public NoAvaliableEmployeeException() : base($"There is no avaliable employee")
        {         
        }
    }
}
