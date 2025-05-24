using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Enums
{
    public class ClientEntryStatus
    {
        public enum EntryStatus
        {
            Success = 0,
            AlreadyUnsubscribed = 1,
            PastEvent = 2
        }
    }
}
