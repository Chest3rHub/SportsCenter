using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class EmployeeDismissedException : Exception
    {
        public int EmployeeId { get; set; }

        public EmployeeDismissedException(int id) : base($"Employee with id: {id} is dismissed.")
        {
            EmployeeId = id;
        }
    }
}
