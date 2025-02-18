using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface INewsRepository
    {
        Task AddNewsAsync(Aktualnosci news, CancellationToken cancellationToken);
        Task<Aktualnosci?> GetNewsByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateNewsAsync(Aktualnosci news, CancellationToken cancellationToken);
        Task RemoveNewsAsync(Aktualnosci news, CancellationToken cancellationToken); 
    }
}
