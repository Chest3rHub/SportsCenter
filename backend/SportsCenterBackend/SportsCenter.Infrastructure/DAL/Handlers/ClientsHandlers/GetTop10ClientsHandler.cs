using MediatR;
using SportsCenter.Application.Clients.Queries.GetTop10Clients;
using Microsoft.EntityFrameworkCore;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers
{
    internal class GetTop10ClientsHandler : IRequestHandler<GetTop10Clients, IEnumerable<Top10ClientDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetTop10ClientsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Top10ClientDto>> Handle(GetTop10Clients request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Klients
                .Include(k => k.KlientNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Text))
            {
                var normalizedText = request.Text.ToLower();
                query = query.Where(k => k.KlientNavigation.Email.ToLower().StartsWith(normalizedText));
            }

            var clients = await query
                .OrderBy(k => k.KlientNavigation.Email)
                .Take(10)
                .Select(k => new Top10ClientDto
                {
                    ClientId = k.KlientId,
                    Email = k.KlientNavigation.Email
                })
                .ToListAsync(cancellationToken);

            return clients;
        }
    }
}
