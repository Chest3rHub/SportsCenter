using SportsCenter.Core.Entities;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsCenter.Application.Orders.Queries.GetOrdersToProcess;
using Stripe.Climate;

namespace SportsCenter.Infrastructure.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private SportsCenterDbContext _dbContext;

        public OrderRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddOrderAsync(Zamowienie order, CancellationToken cancellationToken)
        {
            await _dbContext.Zamowienies.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<Zamowienie> GetActiveOrderByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Zamowienies
                .Include(z => z.ZamowienieProdukts)
                .FirstOrDefaultAsync(z => z.KlientId == userId && z.Status == "Koszyk", cancellationToken);
        }
        public async Task UpdateOrderAsync(Zamowienie order, CancellationToken cancellationToken)
        {
            _dbContext.Zamowienies.Update(order);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<ZamowienieProdukt> GetOrderProductAsync(int orderId, int productId, CancellationToken cancellationToken)
        {
            return await _dbContext.ZamowienieProdukts
                .FirstOrDefaultAsync(op => op.ZamowienieId == orderId && op.ProduktId == productId, cancellationToken);
        }
        public async Task UpdateOrderProductAsync(ZamowienieProdukt orderProduct, CancellationToken cancellationToken)
        {
            _dbContext.ZamowienieProdukts.Update(orderProduct);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task AddOrderProductAsync(ZamowienieProdukt orderProduct, CancellationToken cancellationToken)
        {
            await _dbContext.ZamowienieProdukts.AddAsync(orderProduct, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task RemoveOrderProductAsync(ZamowienieProdukt orderProduct, CancellationToken cancellationToken)
        {
            _dbContext.ZamowienieProdukts.Remove(orderProduct);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<int> RemoveOrderProductAsync(ZamowienieProdukt orderProduct, int quantity, CancellationToken cancellationToken)
        {
            var existingOrderProduct = await _dbContext.ZamowienieProdukts
                .FirstOrDefaultAsync(zp => zp.ProduktId == orderProduct.ProduktId && zp.ZamowienieId == orderProduct.ZamowienieId, cancellationToken);

            if (existingOrderProduct.Liczba < quantity)
            {
                return 0;
            }

            if (existingOrderProduct.Liczba == quantity)
            {
                _dbContext.ZamowienieProdukts.Remove(existingOrderProduct);
            }
            else
            {
                existingOrderProduct.Liczba -= quantity;
                _dbContext.ZamowienieProdukts.Update(existingOrderProduct);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return 1;
        }

        public async Task<decimal> GetTotalOrderCostAsync(int orderId, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Zamowienies
                .Where(z => z.ZamowienieId == orderId)
                .Include(z => z.ZamowienieProdukts)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null || !order.ZamowienieProdukts.Any())
            {
                return 0m;
            }

            decimal totalCost = order.ZamowienieProdukts.Sum(op => op.Koszt);

            return totalCost;
        }
        public async Task RemoveOrderAsync(Zamowienie order, CancellationToken cancellationToken)
        {
            _dbContext.Zamowienies.Remove(order);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<ZamowienieProdukt>> GetOrderProductsAsync(int orderId, CancellationToken cancellationToken)
        {
            return await _dbContext.ZamowienieProdukts
                .Where(op => op.ZamowienieId == orderId)
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Zamowienie>> GetOrdersByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            return await _dbContext.Zamowienies
                .Where(o => o.PracownikId == employeeId)
                .ToListAsync(cancellationToken);
        }
        public async Task<Zamowienie> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken)
        {
            return await _dbContext.Zamowienies.FindAsync(orderId);
        }
    }
}
