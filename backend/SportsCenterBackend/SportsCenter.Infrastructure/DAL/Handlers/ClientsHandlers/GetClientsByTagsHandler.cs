﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Clients.Queries.GetClientsByTags;

namespace SportsCenter.Infrastructure.DAL.Handlers.ClientsHandlers
{
    internal class GetClientsByTagsHandler : IRequestHandler<GetClientsByTags, IEnumerable<ClientByTagsDto>>
    {
        private readonly SportsCenterDbContext _dbContext;

        public GetClientsByTagsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ClientByTagsDto>> Handle(GetClientsByTags request, CancellationToken cancellationToken)
        {
            return await _dbContext.Klients
                .Include(x => x.KlientNavigation)
                .Where(k => request.TagIds.All(tagId => k.Tags.Any(t => t.TagId == tagId)))
                .Select(k => new ClientByTagsDto
                {
                    FullName = k.KlientNavigation.Nazwisko,
                    Email = k.KlientNavigation.Email,
                    Tags = k.Tags.Select(t => new TagDto
                    {
                        TagId = t.TagId,
                        Nazwa = t.Nazwa
                    }).ToList()
                })
                .AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
