using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Clients.Queries.GetClientByEmail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers
{
    internal class GetClientByEmailHandler : IRequestHandler<GetClientByEmail, ClientDto>
    {
        private readonly SportsCenterDbContext _dbContext;
        public GetClientByEmailHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ClientDto> Handle(GetClientByEmail request, CancellationToken cancellationToken)
        {
            return await _dbContext.Klients
                .Include(k => k.KlientNavigation)
                .Where(k => k.KlientNavigation.Email == request.Email)
                .Select(k => new ClientDto
                {
                    CLientId = k.KlientId,
                    Name = k.KlientNavigation.Imie,
                    Surname = k.KlientNavigation.Nazwisko,
                    Email = k.KlientNavigation.Email,
                    DateOfBirth = (DateOnly)k.KlientNavigation.DataUr,
                    Address = k.KlientNavigation.Adres,
                    AccountBalance = k.Saldo,
                    ProductsDiscount = (int)k.ZnizkaNaProdukty,
                    ActivityDiscout = (int)k.ZnizkaNaZajecia
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

        }
    }
}
