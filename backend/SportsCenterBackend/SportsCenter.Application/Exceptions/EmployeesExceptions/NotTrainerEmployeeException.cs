using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public sealed class NotTrainerEmployeeException : Exception
    {
        public int Id { get; set; }

        public NotTrainerEmployeeException(int id) : base($"Employee with id: {id} in not a trainer")
        {
            Id = id;
        }
    }
}
