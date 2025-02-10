using SportsCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Core.Repositories
{
    public interface IProductRepository
    {
        Task AddProductAsync(Produkt product, CancellationToken cancellationToken);
        Task<Produkt?> GetProductByIdAsync(int id, CancellationToken cancellationToken);
        Task UpdateProductAsync(Produkt product, CancellationToken cancellationToken);
        Task DeleteProductAsync(Produkt product, CancellationToken cancellationToken);
        Task<List<Produkt>> GetAllProductsAsync(CancellationToken cancellationToken);
    }
}
