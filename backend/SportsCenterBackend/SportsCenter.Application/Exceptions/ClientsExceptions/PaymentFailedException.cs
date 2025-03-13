using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.EmployeesExceptions
{
    public class PaymentFailedException : Exception
    {
        public PaymentFailedException() : base($"Payment failed.")
        {
        }
    }
}
