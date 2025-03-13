using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Exceptions.TagsException
{
    public class TagAlreadyExistException : Exception
    {
        public string TagName;
        public TagAlreadyExistException(string tagName) : base($"Tag with name: {tagName} already exist in database")
        {
           TagName = tagName;
        }
    }
}
