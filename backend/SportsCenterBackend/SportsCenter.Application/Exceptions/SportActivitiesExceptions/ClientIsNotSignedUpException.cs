using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ClientIsNotSignedUpException : Exception
    {
        public int Id { get; set; }

        public ClientIsNotSignedUpException(int id) : base($"Client with id: {id} is not signed up to this activity")
        {
            Id = id;
        }
    }
}
