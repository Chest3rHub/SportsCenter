using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ClientsExceptions
{
    public class ActivityAlreadyPaidException : Exception
    {
        public int Id;
        public ActivityAlreadyPaidException(int id) : base($"SportActivity with id: {id} already paid")
        {
            Id = id;
        }
    }
}
