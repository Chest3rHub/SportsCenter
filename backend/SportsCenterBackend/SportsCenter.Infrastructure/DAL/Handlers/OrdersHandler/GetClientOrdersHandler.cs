using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Orders.Queries.GetClientOrders;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.OrdersHandler
{
    internal class GetClientOrdersHandler : IRequestHandler<GetClientOrders, IEnumerable<ClientOrdersDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetClientOrdersHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<ClientOrdersDto>> Handle(GetClientOrders request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int clientId))
            {
                throw new UnauthorizedAccessException("You cannot access the orders without logging in.");
            }

            int pageSize = 6;
            int numberPerPage = 7;

            var orders = await _dbContext.Zamowienies
                .Where(z => z.KlientId == clientId)
                .Join(_dbContext.ZamowienieProdukts, z => z.ZamowienieId, zp => zp.ZamowienieId, (z, zp) => new { z, zp })
                .Join(_dbContext.Produkts, joined => joined.zp.ProduktId, p => p.ProduktId, (joined, p) => new ClientOrdersDto
                {
                    OrderId = joined.z.ZamowienieId,
                    ProductName = p.Nazwa,
                    Quantity = joined.zp.Liczba,
                    Cost = joined.zp.Koszt,
                    OrderDate = joined.z.Data,
                    CompletionDate = joined.z.DataRealizacji.HasValue ? (DateOnly)joined.z.DataRealizacji : DateOnly.MinValue,                
                    Status = joined.z.Status
                })
                .OrderByDescending(o => o.OrderDate)
                .Skip(request.Offset * pageSize)
                .Take(numberPerPage)
                .ToListAsync(cancellationToken);

            return orders;
        }
    }
}
