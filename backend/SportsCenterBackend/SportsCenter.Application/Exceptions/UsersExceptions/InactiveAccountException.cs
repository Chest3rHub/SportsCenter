using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.UsersExceptions
{
    public class InactiveAccountException : Exception
    {
        public InactiveAccountException() : base("The account is inactive") { }
    }
}
