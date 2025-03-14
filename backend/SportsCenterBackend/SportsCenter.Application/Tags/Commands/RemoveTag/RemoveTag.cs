using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Tags.Commands.RemoveTag
{
    public sealed record RemoveTag : ICommand<Unit>
    {
        public int TagId { get; set; }
        public RemoveTag(int tagId)
        {
            TagId = tagId;
        }
    }
}
