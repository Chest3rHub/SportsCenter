using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Products.Queries.GetCartProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ProductsHandlers
{
    internal class GetCartProductsHandler : IRequestHandler<GetCartProducts, IEnumerable<CartProductDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetCartProductsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<CartProductDto>> Handle(GetCartProducts request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Zamowienies
                .Where(z => z.KlientId == request.ClientId && z.Status == "Koszyk")
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                return new List<CartProductDto>();
            }

            var orderProducts = await _dbContext.ZamowienieProdukts
                .Where(op => op.ZamowienieId == order.ZamowienieId)
                .Include(op => op.Produkt)
                .ToListAsync(cancellationToken);

            var cartProducts = orderProducts.Select(op => new CartProductDto
            {
                ProductId = op.ProduktId,
                Name = op.Produkt.Nazwa,
                Quantity = op.Liczba,
                Cost = op.Koszt
            }).ToList();

            return cartProducts;
        }
    }
}
