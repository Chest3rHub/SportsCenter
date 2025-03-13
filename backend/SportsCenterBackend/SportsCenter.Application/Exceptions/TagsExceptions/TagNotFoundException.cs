using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.TagsException
{
    public class TagNotFoundException : Exception
    {
        public int Id;
        public TagNotFoundException(int id) : base($"Tag with id: {id} not found")
        {
            Id = id;
        }
    }
}
