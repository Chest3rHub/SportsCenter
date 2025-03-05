using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class AbsenceRequestAlreadyAcceptedException : Exception
    {
        public int Id { get; set; }

        public AbsenceRequestAlreadyAcceptedException(int id) : base($"Absence request with id: {id} is already accpeted")
        {
            Id = id;
        }
    }
}
