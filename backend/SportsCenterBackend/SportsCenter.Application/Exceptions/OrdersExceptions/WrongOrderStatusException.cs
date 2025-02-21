using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.OrdersExceptions
{
    public sealed class WrongOrderStatusException : Exception
    {
        string StatusName { get; set; }
        public WrongOrderStatusException(string statusName) : base($"Order must be in {statusName} to complete the action.")
        {
            StatusName = statusName;
        }
    }
}
