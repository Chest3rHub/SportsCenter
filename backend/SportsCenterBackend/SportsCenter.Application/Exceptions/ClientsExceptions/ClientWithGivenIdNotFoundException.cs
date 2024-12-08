using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ClientsExceptions
{
    public sealed class ClientWithGivenIdNotFoundException : Exception
    {
        public int Id { get; set; }

        public ClientWithGivenIdNotFoundException(int id) : base($"Client with id {id} not found")
        {
            Id = id;
        }
    }
}
