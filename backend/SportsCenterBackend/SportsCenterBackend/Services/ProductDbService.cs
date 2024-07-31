using SportsCenterBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCenterBackend.Context;

namespace SportsCenterBackend.Services
{
    public class ProductDbService : IProductDbService
    {
        private readonly SportsCenterDbContext _context;

        public ProductDbService(SportsCenterDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Produkt product)
        {
            _context.Produkts.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int idProduct)
        {
            var product = await _context.Produkts.FindAsync(idProduct);
            if (product != null)
            {
                _context.Produkts.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Produkt>> GetProductsAsync()
        {
            return await _context.Produkts.ToListAsync();
        }
    }
}

