using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.ProductsExceptions
{
    public sealed class NoActiveOrdersForCLientException : Exception
    {
        public int Id { get; set; }

        public NoActiveOrdersForCLientException(int id) : base($"there is no active orders for client with id: {id}")
        {
            Id = id;
        }
    }
}
