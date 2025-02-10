using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Queries.GetProducts
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}
