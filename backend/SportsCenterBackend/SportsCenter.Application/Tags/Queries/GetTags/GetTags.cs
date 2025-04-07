using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Tags.Queries.GetTags
{
    public class GetTags : IQuery<IEnumerable<TagDto>>
    {
        public int Offset { get; set; } = 0;
        public GetTags(int offSet)
        {
            Offset = offSet;
        }
    }
}
