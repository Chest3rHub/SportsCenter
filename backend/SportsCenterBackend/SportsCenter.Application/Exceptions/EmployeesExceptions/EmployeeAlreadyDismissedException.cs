using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class EmployeeAlreadyDismissedException : Exception
    {
        public int EmployeeId { get; set; }
       
        public EmployeeAlreadyDismissedException(int id) : base($"Employee with id: {id} already dismissed.")
        {
            EmployeeId = id;
        }
    }
}
