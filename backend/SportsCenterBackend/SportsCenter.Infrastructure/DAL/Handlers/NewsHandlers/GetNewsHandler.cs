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
            // zwraca 4 ale wyswietlane sa tylko 3 zeby bylo wiadomo czy odpytywac ze zwiekszonym 
            // offsetem czy juz wiecej rekordow nie ma wiec sie nie da i tak
            int pageSize = 3;
            int numberPerPage = 4;

            var newsList = await _dbContext.Aktualnoscis
                .Where(news =>  DateOnly.FromDateTime(news.WazneDo.Value) >= today || !news.WazneDo.HasValue)
                .OrderByDescending(news => news.WazneOd)
                .Skip(request.Offset * pageSize)
                .Take(numberPerPage)
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
