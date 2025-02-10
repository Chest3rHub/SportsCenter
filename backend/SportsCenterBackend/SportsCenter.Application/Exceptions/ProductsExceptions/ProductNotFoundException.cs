using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ProductsExceptions
{
    public sealed class ProductNotFoundException : Exception
    {
        public int Id { get; set; }

        public ProductNotFoundException(int id) : base($"Product with id: {id} not found")
        {
            Id = id;
        }
    }
}
