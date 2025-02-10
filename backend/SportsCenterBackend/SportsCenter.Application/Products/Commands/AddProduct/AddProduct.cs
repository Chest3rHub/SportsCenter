using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.AddProduct
{
    public sealed record AddProduct : ICommand<Unit>
    {  
        public string Name { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;

        public AddProduct(string name, string manufacturer, int stockQuantity, decimal price, string imageUrl) {        
            Name = name;
            Manufacturer = manufacturer;
            StockQuantity = stockQuantity;
            Price = price;
            ImageUrl = imageUrl;
        }
    }
}
