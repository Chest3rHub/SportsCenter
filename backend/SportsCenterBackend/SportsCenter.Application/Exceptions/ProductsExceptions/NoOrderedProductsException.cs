using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ProductsExceptions
{
    public sealed class NoOrderedProductsException : Exception
    {
        public NoOrderedProductsException() : base($"there is no products ordered")
        {
        }
    }
}
