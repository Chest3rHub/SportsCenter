using MediatR.NotificationPublishers;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Queries.GetClientByEmail
{
    public class GetClientByEmail : IQuery<ClientDto>
    {
       public string Email { get; set; }
        public GetClientByEmail(string email)
        {
            Email = email;
        }
    }
}
