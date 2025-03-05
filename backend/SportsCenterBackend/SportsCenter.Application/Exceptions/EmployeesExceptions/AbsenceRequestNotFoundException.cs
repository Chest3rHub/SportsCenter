using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class AbsenceRequestNotFoundException : Exception
    {
        public int Id { get; set; }

        public AbsenceRequestNotFoundException(int id) : base($"Absence request with id: {id} not found")
        {
            Id = id;
        }
    }
}
