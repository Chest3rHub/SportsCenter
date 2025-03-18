using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Clients.Queries.GetClients;
using SportsCenter.Application.News.Queries.GetNews;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.NewsHandlers
{
    internal class GetNewsHandler : IRequestHandler<GetNews, IEnumerable<NewsDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetNewsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<NewsDto>> Handle(GetNews request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var newsList = await _dbContext.Aktualnoscis
                .Where(news =>  DateOnly.FromDateTime(news.WazneDo.Value) >= today)
                .Select(news => new NewsDto
                {
                    Id = news.AktualnosciId,
                    Title = news.Nazwa,
                    Content = news.Opis,
                    ValidFrom = news.WazneOd,
                    ValidUntil = news.WazneDo
                })
                .ToListAsync(cancellationToken);

            return newsList;
        }
    }
}
