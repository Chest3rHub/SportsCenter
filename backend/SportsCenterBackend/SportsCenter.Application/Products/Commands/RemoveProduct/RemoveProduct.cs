using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.RemoveProduct
{
    public sealed record RemoveProduct : ICommand<Unit>
    {
        public int ProductId { get; set; }

        public RemoveProduct(int productId)
        {
            ProductId = productId;
        }
    }
}
