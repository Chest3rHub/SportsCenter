using MediatR;
using SportsCenter.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Clients.Commands.RemoveClientTags;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers;

internal sealed class RemoveClientTagsHandler : IRequestHandler<RemoveClientTags, Unit>
{
    private readonly SportsCenterDbContext _dbContext;

    public RemoveClientTagsHandler(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(RemoveClientTags request, CancellationToken cancellationToken)
    {
        var client = await _dbContext.Klients
            .Include(k => k.Tags)
            .FirstOrDefaultAsync(k => k.KlientId == request.ClientId, cancellationToken);

        if (client == null)
            throw new ClientNotFoundException("Client with ID -> " + request.ClientId.ToString() + " not found");

        var tagsToRemove = client.Tags
            .Where(tag => request.TagIds.Contains(tag.TagId))
            .ToList();

        foreach (var tag in tagsToRemove)
        {
            client.Tags.Remove(tag);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
