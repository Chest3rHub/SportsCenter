using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Products.Queries.GetProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ProductsHandlers
{
    internal class GetProductsHandler : IRequestHandler<GetProducts, IEnumerable<ProductDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetProductsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<ProductDto>> Handle(GetProducts request, CancellationToken cancellationToken)
        {          
            var products = await _dbContext.Produkts
                .Select(p => new ProductDto
                {                 
                    Name = p.Nazwa,
                    Manufacturer = p.Producent,
                    StockQuantity = p.LiczbaNaStanie,
                    Price = p.Koszt
                })
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}
