using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Queries.GetClientOrders
{
    public class ClientOrdersDto
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public DateOnly OrderDate { get; set; }
        public string Status { get; set; }
        public DateOnly CompletionDate { get; set; }

    }
}
