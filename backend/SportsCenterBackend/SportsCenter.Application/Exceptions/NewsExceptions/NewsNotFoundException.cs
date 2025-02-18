using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.NewsExceptions
{
    public sealed class NewsNotFoundException : Exception
    {
        public int Id { get; set; }

        public NewsNotFoundException(int id) : base($"News with id {id} not found")
        {
            Id = id;
        }
    }
}
