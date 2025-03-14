using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.Tags.Commands.AddTag
{
    public sealed record AddTag : ICommand<Unit>
    {
        public string TagName { get; set; }

        public AddTag(string tagName)
        {
            TagName = tagName;
        }
    }
}
