using SportsCenter.Application.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Orders.Queries.GetOrdersToProcess
{
    public class GetOrdersToProcess : IQuery<IEnumerable<OrdersToProcessDto>>
    {
    }
}
