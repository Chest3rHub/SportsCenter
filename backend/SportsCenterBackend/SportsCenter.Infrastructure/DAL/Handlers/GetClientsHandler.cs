using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Users.Queries;
using SportsCenter.Application.Users.Queries.GetClients;
using SportsCenterBackend.Context;

namespace SportsCenter.Infrastructure.DAL.Handlers;

internal class GetClientsHandler : IRequestHandler<GetClients, IEnumerable<ClientDto>>
{
    private readonly SportsCenterDbContext _dbContext;

    public GetClientsHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ClientDto>> Handle(GetClients request, CancellationToken cancellationToken)
    {
        return await _dbContext.Klients.Include(x => x.KlientNavigation)
            .Select(k => new ClientDto
            {
                FullName = k.KlientNavigation.Nazwisko,
                Email = k.KlientNavigation.Email
            }).AsNoTracking().ToListAsync(cancellationToken);
    }
}