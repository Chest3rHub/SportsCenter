using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.OrdersExceptions
{
    public sealed class OrderAlreadyProcessedException : Exception
    {
        public int Id { get; set; }

        public OrderAlreadyProcessedException(int id) : base($"Order with id: {id} is already processed.")
        {
            Id = id;
        }
    }
}
