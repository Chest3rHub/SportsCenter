using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesException
{
    public sealed class EmployeeNotFoundException : Exception
    {
        public int Id { get; set; }

        public EmployeeNotFoundException(int id) : base($"Employee with id: {id} not found")
        {
            Id = id;
        }
    }
}
