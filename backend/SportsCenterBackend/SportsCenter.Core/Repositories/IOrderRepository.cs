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
        Task<int> RemoveOrderProductAsync(ZamowienieProdukt orderProduct, int quantity, CancellationToken cancellationToken);
        Task<decimal> GetTotalOrderCostAsync(int orderId, CancellationToken cancellationToken);
        Task RemoveOrderAsync(Zamowienie order, CancellationToken cancellationToken);
        Task<List<ZamowienieProdukt>> GetOrderProductsAsync(int orderId, CancellationToken cancellationToken);
        Task<IEnumerable<Zamowienie>> GetOrdersByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<Zamowienie> GetOrderByIdAsync (int orderId, CancellationToken cancellationToken);
    }
}
