using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ClientsExceptions
{
    public class ClientWithdrawnException : Exception
    {
        public ClientWithdrawnException() : base($"Client withdrew from this activity.")
        {
        }
    }
}
