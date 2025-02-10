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
    public class ProductRepository : IProductRepository
    {
        private SportsCenterDbContext _dbContext;

        public ProductRepository(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddProductAsync(Produkt product, CancellationToken cancellationToken)
        {
            var produkt = new Produkt
            {
                Nazwa = product.Nazwa,
                Producent = product.Producent,
                LiczbaNaStanie = product.LiczbaNaStanie,
                Koszt = product.Koszt,
                ZdjecieUrl = product.ZdjecieUrl
            };
            await _dbContext.AddAsync(produkt, cancellationToken);
        }
        public async Task<Produkt?> GetProductByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Produkts
                .FirstOrDefaultAsync(p => p.ProduktId == id, cancellationToken);
        }
        public async Task UpdateProductAsync(Produkt product, CancellationToken cancellationToken)
        {
            _dbContext.Produkts.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteProductAsync(Produkt product, CancellationToken cancellationToken)
        {
            _dbContext.Produkts.Remove(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<Produkt>> GetAllProductsAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Produkts.ToListAsync(cancellationToken);
        }
    }
}
