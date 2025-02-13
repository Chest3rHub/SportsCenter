using MediatR;
using SportsCenter.Application.Exceptions.ClientsExceptions;
using SportsCenter.Application.Exceptions.TaskExceptions;
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
            throw new ClientWithGivenIdNotFoundException(request.ClientId);

        // Pobranie ID ju¿ przypisanych tagów
        var currentTagIds = client.Tags.Select(t => t.TagId).ToList();

        // Sprawdzenie, czy liczba tagów nie przekroczy limitu (max 3)
        if (currentTagIds.Count + request.TagIds.Count > 3)
            throw new TagLimitException(client.KlientId);

        // Pobranie tagów, które istniej¹ w bazie
        var existingTags = await _tagRepository.GetTagsByIdsAsync(request.TagIds, cancellationToken);
        var existingTagIds = existingTags.Select(t => t.TagId).ToList();

        // Sprawdzenie, czy wszystkie podane tagi istniej¹ w bazie
        var invalidTags = request.TagIds.Except(existingTagIds).ToList();
        if (invalidTags.Any())
            throw new TagNotFoundException();

        // Filtrowanie tagów, które jeszcze nie s¹ przypisane do klienta
        var tagsToAdd = existingTags.Where(t => !currentTagIds.Contains(t.TagId)).ToList();
        if (!tagsToAdd.Any())
            return Unit.Value;

        // Dodanie tagów do klienta
        foreach (var tag in tagsToAdd)
        {
            client.Tags.Add(tag);
        }


        // Aktualizacja klienta w bazie
        await _clientRepository.UpdateClientAsync(client, cancellationToken);

        return Unit.Value; 
    }
}