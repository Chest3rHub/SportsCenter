using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.BuyCartProduct
{
    public sealed record BuyCartProduct : ICommand<Unit>
    {    

        public BuyCartProduct()
        {

        }
    }
}
