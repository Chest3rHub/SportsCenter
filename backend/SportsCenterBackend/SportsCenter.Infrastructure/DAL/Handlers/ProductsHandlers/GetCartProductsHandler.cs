using MediatR;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCartProductsHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<CartProductDto>> Handle(GetCartProducts request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("You cannot access the cart without logging in.");
            }

            var order = await _dbContext.Zamowienies
                .Where(z => z.KlientId == userId && z.Status == "Koszyk")
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
