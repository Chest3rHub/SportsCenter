using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.UsersException
{
    public sealed class UserNotFoundException : Exception
    {
        public int Id { get; set; }

        public UserNotFoundException(int id) : base($"User with id {id} not found")
        {
            Id = id;
        }
    }
}
