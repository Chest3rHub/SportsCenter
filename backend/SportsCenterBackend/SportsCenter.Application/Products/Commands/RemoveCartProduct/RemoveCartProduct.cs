using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.RemoveCartProduct
{
    public sealed record RemoveCartProduct : ICommand<Unit>
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public RemoveCartProduct(int clientId, int productId)
        {
            ClientId = clientId;
            ProductId = productId;
        }
    }
}
