using MediatR;
using SportsCenter.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Clients.Commands.AddClientTags;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers
{
    internal sealed class AddClientTagsHandler : IRequestHandler<AddClientTags, Unit>
    {
        private readonly SportsCenterDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddClientTagsHandler(SportsCenterDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(AddClientTags request, CancellationToken cancellationToken)
        {
            var client = await _dbContext.Klients
                .Include(k => k.Tags)
                .FirstOrDefaultAsync(k => k.KlientId == request.ClientId, cancellationToken);

            if (client == null)
                throw new ClientNotFoundException($"Client with ID -> {request.ClientId} not found");

            int currentTagCount = await new GetClientTagCountHandler(_dbContext)
                .Handle(new GetClientTagCount(request.ClientId), cancellationToken);

            if (currentTagCount + request.TagIds.Count > 3)
                throw new TagLimitException(client.KlientId);

            foreach (var tagId in request.TagIds)
            {
                if (!client.Tags.Any(t => t.TagId == tagId))
                {
                    var tag = await _dbContext.Tags.FirstOrDefaultAsync(t => t.TagId == tagId, cancellationToken);
                    if (tag != null)
                    {
                        client.Tags.Add(tag);
                    }
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
