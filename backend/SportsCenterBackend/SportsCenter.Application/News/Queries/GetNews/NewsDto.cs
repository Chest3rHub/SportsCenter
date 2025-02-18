using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Application.News.Queries.GetNews
{
    public class NewsDto
    {
        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime ValidFrom { get; set; }

        public DateTime? ValidUntil { get; set; }
    }
}
