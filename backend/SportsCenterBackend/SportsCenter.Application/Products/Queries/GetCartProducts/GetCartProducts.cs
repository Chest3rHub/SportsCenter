using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Queries.GetCartProducts
{
    public class GetCartProducts : IQuery<IEnumerable<CartProductDto>>
    {
        public int ClientId { get; set; }
        public GetCartProducts(int clientId)
        {
           ClientId = clientId;
        }
    }
}
