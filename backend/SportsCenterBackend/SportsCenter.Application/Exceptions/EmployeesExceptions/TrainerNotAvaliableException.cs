using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public sealed class TrainerNotAvaliableException : Exception
    {
        public TrainerNotAvaliableException() : base($"Trainer is not avaliable at this time")
        {

        }
    }
}
