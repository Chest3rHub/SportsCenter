﻿using SportsCenter.Core.Entities;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
