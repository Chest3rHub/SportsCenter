using MediatR;
using SportsCenter.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Commands.RemoveNews
{
    public sealed record RemoveNews : ICommand<Unit>
    {
        public int NewsId { get; set; }

        public RemoveNews(int newsId)
        {
            NewsId = newsId;
        }
    }
}
