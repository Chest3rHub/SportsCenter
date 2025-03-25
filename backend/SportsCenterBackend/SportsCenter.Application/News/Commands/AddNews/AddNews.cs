using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.AddNews
{
    public sealed record AddNews : ICommand<Unit>
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime ValidFrom { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ValidUntil { get; set; }

        public AddNews(string title, string content, DateTime validFrom, DateTime? validUntil = null)
        {
            Title = title;
            Content = content;
            ValidFrom = validFrom;
            ValidUntil = validUntil;
        }
    }
}
