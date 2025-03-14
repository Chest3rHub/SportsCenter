using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.SportActivitiesExceptions
{
    public class ClientAlreadySignedUpException : Exception
    {
        public int Id { get; set; }

        public ClientAlreadySignedUpException(int id) : base($"Client with id: {id} is already signed up to this activity")
        {
            Id = id;
        }
    }
}
