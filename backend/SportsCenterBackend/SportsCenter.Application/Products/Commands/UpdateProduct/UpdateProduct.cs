using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Commands.UpdateProduct
{
    public sealed record UpdateProduct : ICommand<Unit>
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;

        public UpdateProduct(int productId, string name, string manufacturer, int stockQuantity, decimal price, string imageUrl)
        {
            ProductId = productId;
            Name = name;
            Manufacturer = manufacturer;
            StockQuantity = stockQuantity;
            Price = price;
            ImageUrl = imageUrl;
        }
    }
}
