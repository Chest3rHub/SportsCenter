using SportsCenterBackend.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCenterBackend.DTOs;

namespace SportsCenterBackend.Services
{
    public interface IProductDbService
    {
        Task<List<ProductDTO>> GetProductsAsync();
        Task<Produkt> AddProductAsync(ProductWithoutIdDTO productDto);
        Task DeleteProductAsync(int idProduct);
    }
}
