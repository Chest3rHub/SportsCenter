using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesException
{
    public sealed class NotAdmEmployeeException : Exception
    {
        public int Id { get; set; }

        public NotAdmEmployeeException(int id) : base($"Employee with id: {id} in not an administrative employee")
        {
            Id = id;
        }
    }
}
