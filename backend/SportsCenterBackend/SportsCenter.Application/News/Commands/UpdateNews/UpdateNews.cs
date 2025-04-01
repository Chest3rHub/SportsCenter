using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.UpdateNews
{
    public sealed record UpdateNews : ICommand<Unit>
    {
        public int NewsId { get; set; }
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime ValidFrom { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ValidUntil { get; set; }

        public UpdateNews(int newsId, string title, string content,  DateTime validFrom, DateTime? validUntil = null)
        {
            NewsId = newsId;
            Title = title;
            Content = content;         
            ValidFrom = validFrom;
            ValidUntil = validUntil;
        }
    }
}
