using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private SportsCenterDbContext _dbContext;

        public ClientRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddClientAsync(Klient client, CancellationToken cancellationToken)
        {
            await _dbContext.Klients.AddAsync(client, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Klient?> GetClientByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Klients
                .Include(k => k.KlientNavigation)
                .FirstOrDefaultAsync(k => k.KlientNavigation.Email == email, cancellationToken);
        }

        public async Task<Klient?> GetClientByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Klients
                .Include(k => k.KlientNavigation)
                .FirstOrDefaultAsync(k => k.KlientNavigation.OsobaId == id, cancellationToken);
        }

        public async Task UpdateClientAsync(Klient client, CancellationToken cancellationToken)
        {
            _dbContext.Klients.Update(client);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Tag?> GetTagById(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Tags.Where(o => o.TagId == id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
        
        public async Task<bool> RemoveClientTagsAsync(int clientId, List<int> tagIds, CancellationToken cancellationToken)
        {
            var client = await _dbContext.Klients
                .Include(c => c.Tags)
                .FirstOrDefaultAsync(c => c.KlientId == clientId, cancellationToken);

            if (client == null)
                return false;
            
            var tagsToRemove = client.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
            if (!tagsToRemove.Any())
                return false;
            
            foreach (var tag in tagsToRemove)
            {
                client.Tags.Remove(tag); 
            }
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
