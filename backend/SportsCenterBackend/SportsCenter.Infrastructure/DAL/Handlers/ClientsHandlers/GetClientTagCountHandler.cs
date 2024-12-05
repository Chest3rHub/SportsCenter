using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Clients.Commands.AddClientTags;
using System.Threading;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers
{
    internal class GetClientTagCountHandler : IRequestHandler<GetClientTagCount, int>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetClientTagCountHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetClientTagCount request, CancellationToken cancellationToken)
        {
            return await _dbContext.Klients
                .Include(x => x.KlientNavigation)
                .Where(k => k.KlientId == request.ClientId)
                .Select(k => k.Tags.Count)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}