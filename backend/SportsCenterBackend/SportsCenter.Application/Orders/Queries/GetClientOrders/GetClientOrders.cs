using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Queries.GetClientOrders
{
    public class GetClientOrders : IQuery<IEnumerable<ClientOrdersDto>>
    {
        public int Offset { get; set; } = 0;
        public GetClientOrders(int offSet)
        {
            Offset = offSet;
        }
    }
}
