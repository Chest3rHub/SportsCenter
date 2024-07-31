using SportsCenterBackend.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportsCenterBackend.Services
{
    public interface IProductDbService
    {
        Task<List<Produkt>> GetProductsAsync();
        Task AddProductAsync(Produkt product);
        Task DeleteProductAsync(int idProduct);
    }
}
