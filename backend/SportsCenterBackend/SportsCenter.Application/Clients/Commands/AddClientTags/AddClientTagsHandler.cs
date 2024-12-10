using MediatR;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Application.Clients.Commands.AddClientTags;

internal sealed class AddClientTagsHandler : IRequestHandler<AddClientTags, Unit>
{
    private readonly IClientRepository _clientRepository;
    private readonly ITagRepository _tagRepository;

    public AddClientTagsHandler(IClientRepository clientRepository, ITagRepository tagRepository)
    {
        _clientRepository = clientRepository;
        _tagRepository = tagRepository;
    }

    public async Task<Unit> Handle(AddClientTags request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByIdAsync(request.ClientId, cancellationToken);
        if (client == null)
            throw new ClientNotFoundException($"Client with ID -> {request.ClientId} not found");
        
        var currentTags = client.Tags.Select(t => t.TagId).ToList();
        if (currentTags.Count + request.TagIds.Count > 3)
            throw new TagLimitException(client.KlientId);
        
        var tagsToAdd = request.TagIds.Except(currentTags).ToList();
        if (!tagsToAdd.Any())
            return Unit.Value;
        
        var tags = await _tagRepository.GetTagsByIdsAsync(tagsToAdd, cancellationToken);
        
        foreach (var tag in tags)
        {
            client.Tags.Add(tag);
        }
        
        await _clientRepository.UpdateClientAsync(client, cancellationToken);

        return Unit.Value;
    }
}