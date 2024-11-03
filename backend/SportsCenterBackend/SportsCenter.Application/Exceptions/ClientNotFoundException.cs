using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions
{
    public sealed class ClientNotFoundException : Exception
    {
        public string Email { get; set; }

        public ClientNotFoundException(string email) : base($"Client with email {email} not found")
        {
            Email = email;
        }
    }
}
