using SportsCenterBackend.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCenterBackend.Context;
using SportsCenterBackend.DTOs;

namespace SportsCenterBackend.Services
{
    public class ProductDbService : IProductDbService
    {
        private readonly SportsCenterDbContext _context;

        public ProductDbService(SportsCenterDbContext context)
        {
            _context = context;
        }

        public async Task<Produkt> AddProductAsync(ProductWithoutIdDTO productDto)
        {
            // tutaj tez walidacja czy nie ma juz produktu np o tej nazwie moze?
            var product = new Produkt()
            {
                Nazwa = productDto.Nazwa,
                Producent = productDto.Producent,
                Ilosc = productDto.Ilosc,
                Koszt = productDto.Koszt,
                Zdjecie = productDto.Zdjecie,
            };
            _context.Produkts.Add(product);
            await _context.SaveChangesAsync();
            return product;
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

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            var productsList = await _context.Produkts.ToListAsync();
            var productsDtoList = new List<ProductDTO>();
            productsList.ForEach(product => productsDtoList.Add(new ProductDTO()
            {
                ProduktId = product.ProduktId,
                Nazwa = product.Nazwa,
                Producent = product.Producent,
                Ilosc = product.Ilosc,
                Koszt = product.Koszt,
                Zdjecie = product.Zdjecie,
            }));
            return productsDtoList;
        }
    }
}

