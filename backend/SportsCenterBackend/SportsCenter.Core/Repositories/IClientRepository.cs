using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface IClientRepository
    {
        Task AddClientAsync(Klient client, CancellationToken cancellationToken);
        Task<Klient?> GetClientByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Klient?> GetClientByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateClientAsync(Klient client, CancellationToken cancellationToken);

        Task<Tag?> GetTagById(int id, CancellationToken cancellationToken);
    }
}
