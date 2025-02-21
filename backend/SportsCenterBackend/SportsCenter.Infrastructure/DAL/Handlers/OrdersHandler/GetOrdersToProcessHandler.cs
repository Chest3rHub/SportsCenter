using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Orders.Queries.GetOrdersToProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.OrdersHandler
{
    internal class GetOrdersToProcessHandler : IRequestHandler<GetOrdersToProcess, IEnumerable<OrdersToProcessDto>>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetOrdersToProcessHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<OrdersToProcessDto>> Handle(GetOrdersToProcess request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int employeeId))
            {
                throw new UnauthorizedAccessException("You cannot access the orders without logging in.");
            }      

            var orders = await _dbContext.Zamowienies
                .Where(z => z.PracownikId == employeeId && z.Status != "Zrealizowane")
                .Join(_dbContext.ZamowienieProdukts, z => z.ZamowienieId, zp => zp.ZamowienieId, (z, zp) => new { z, zp })
                .Join(_dbContext.Produkts, joined => joined.zp.ProduktId, p => p.ProduktId, (joined, p) => new { joined, p })
                .Join(_dbContext.Klients, joined => joined.joined.z.KlientId, k => k.KlientId, (joined, k) => new OrdersToProcessDto
                {
                    ProductName = joined.p.Nazwa,
                    Quantity = joined.joined.zp.Liczba,
                    Cost = joined.joined.zp.Koszt,
                    OrderDate = joined.joined.z.Data,
                    ClientFirstName = k.KlientNavigation.Imie,
                    ClientLastName = k.KlientNavigation.Nazwisko
                })
                .ToListAsync(cancellationToken);
            return orders;
        }
    }
}