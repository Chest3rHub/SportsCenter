using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Clients.Queries.GetClientByEmail
{
    public class ClientDto
    {
        public int CLientId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; }
        public decimal AccountBalance { get; set; }
        public int ProductsDiscount {  get; set; }
        public int ActivityDiscout { get; set; }

    }
}
