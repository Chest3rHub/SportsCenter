using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ProductsExceptions
{
    public sealed class NotEnoughProductInStockException : Exception
    {
        public int Id { get; set; }

        public NotEnoughProductInStockException(int id) : base($"not enough product with id number {id} in stock")
        {
            Id = id;
        }
    }
}
