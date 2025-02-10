using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Zamowienie order, CancellationToken cancellationToken);
        Task<Zamowienie> GetActiveOrderByUserIdAsync(int userId, CancellationToken cancellationToken);     
        Task UpdateOrderAsync(Zamowienie order, CancellationToken cancellationToken);
        Task<ZamowienieProdukt> GetOrderProductAsync(int orderId, int productId, CancellationToken cancellationToken);
        Task UpdateOrderProductAsync(ZamowienieProdukt orderProduct, CancellationToken cancellationToken);
        Task AddOrderProductAsync(ZamowienieProdukt orderProduct, CancellationToken cancellationToken);
        Task RemoveOrderProductAsync(ZamowienieProdukt orderProduct, CancellationToken cancellationToken);
    }
}
