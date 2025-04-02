using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Products.Queries.GetProducts
{
    public class GetProducts : IQuery<IEnumerable<ProductDto>>
    {
        public int Offset { get; set; } = 0;
        public GetProducts(int offSet)
        {
            Offset = offSet;
        }
    }
}
