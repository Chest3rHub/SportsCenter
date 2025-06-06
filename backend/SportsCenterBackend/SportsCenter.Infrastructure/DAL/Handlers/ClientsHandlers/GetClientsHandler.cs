using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Clients.Queries.GetClients;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers;

internal class GetClientsHandler : IRequestHandler<GetClients, IEnumerable<ClientDto>>
{
    private readonly SportsCenterDbContext _dbContext;

    public GetClientsHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ClientDto>> Handle(GetClients request, CancellationToken cancellationToken)
    {
        int PageSize = 6;
        int NumberPerPage = 7;

        return await _dbContext.Klients
       .Include(x => x.KlientNavigation)
       .Select(k => new ClientDto
       {
           Id = k.KlientNavigation.OsobaId,
           Name = k.KlientNavigation.Imie,
           Surname = k.KlientNavigation.Nazwisko,
           Email = k.KlientNavigation.Email
       })
       .AsNoTracking()
       .Skip(request.Offset * PageSize)
       .Take(NumberPerPage)
       .ToListAsync(cancellationToken);
    }
}