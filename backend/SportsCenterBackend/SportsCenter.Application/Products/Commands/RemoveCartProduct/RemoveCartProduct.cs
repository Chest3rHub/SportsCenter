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
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public RemoveCartProduct(int productId, int quantity)
        {          
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
