using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Queries.GetOrdersToProcess
{
    public class OrdersToProcessDto
    {     
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public DateOnly OrderDate { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
    }
}
