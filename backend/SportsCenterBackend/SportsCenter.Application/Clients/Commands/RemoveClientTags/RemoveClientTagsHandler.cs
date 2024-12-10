using MediatR;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Application.Clients.Commands.RemoveClientTags;

internal sealed class RemoveClientTagsHandler : IRequestHandler<RemoveClientTags, Unit>
{
    private readonly IClientRepository _clientRepository;

    public RemoveClientTagsHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Unit> Handle(RemoveClientTags request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByIdAsync(request.ClientId, cancellationToken);
        if (client == null)
            throw new ClientNotFoundException($"Client with ID -> {request.ClientId} not found");
        
        bool result = await _clientRepository.RemoveClientTagsAsync(request.ClientId, request.TagIds, cancellationToken);
        if (!result)
            throw new InvalidOperationException("There are no avaiable tags to remove");

        return Unit.Value;
    }
}


