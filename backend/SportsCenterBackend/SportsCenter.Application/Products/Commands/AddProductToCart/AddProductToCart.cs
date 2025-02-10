using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.AddProductToCart
{
    public sealed record AddProductToCart : ICommand<Unit>
    {
        public int ProductId { get; set; }
       // public string Name { get; set; } = null!;      
        public int Quantity { get; set; }
       // public decimal Price { get; set; }      
        public AddProductToCart(int productId, int quantity)
        {
            ProductId = productId;
          //  Name = name;           
            Quantity = quantity;
          //  Price = price;          
        }
    }
}
