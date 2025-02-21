using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.OrdersExceptions
{
    public sealed class OrderNotFoundException : Exception
    {
        public int Id { get; set; }

        public OrderNotFoundException(int id) : base($"Order with id: {id} not found")
        {
            Id = id;
        }
    }
}
