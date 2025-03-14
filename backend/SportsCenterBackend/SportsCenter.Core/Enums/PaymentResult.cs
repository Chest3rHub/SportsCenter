using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Enums
{
    public class PaymentResult
    {
        public enum PaymentResultEnum
        {
            Success,
            InsufficientFunds,
            ActivityNotFound,
            ActivityInstanceNotFound,
            AlreadyPaid,
            ActivityCanceled,
            ClientNotFound,
            ScheduleNotFound,
            ClientWithdrawn
        }
    }
}
