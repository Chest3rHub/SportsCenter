using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Exceptions.NewsExceptions;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private SportsCenterDbContext _dbContext;

        public NewsRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddNewsAsync(Aktualnosci news, CancellationToken cancellationToken)
        {
            await _dbContext.Aktualnoscis.AddAsync(news, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Aktualnosci?> GetNewsByIdAsync(int id, CancellationToken cancellationToken)
        {      
            return await _dbContext.Aktualnoscis
                .FirstOrDefaultAsync(news => news.AktualnosciId == id, cancellationToken);
        }
        public async Task UpdateNewsAsync(Aktualnosci news, CancellationToken cancellationToken)
        {          
            _dbContext.Aktualnoscis.Update(news);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task RemoveNewsAsync(Aktualnosci news, CancellationToken cancellationToken)
        {          
            _dbContext.Aktualnoscis.Remove(news);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
