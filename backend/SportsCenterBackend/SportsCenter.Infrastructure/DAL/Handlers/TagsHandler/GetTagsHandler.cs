using MediatR;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Application.Clients.Queries.GetClients;
using SportsCenter.Application.Tags.Queries.GetTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsCenter.Infrastructure.DAL.Handlers.TagsHandler
{
    internal class GetTagsHandler : IRequestHandler<GetTags, IEnumerable<TagDto>>
    {

        private readonly SportsCenterDbContext _dbContext;
        public GetTagsHandler(SportsCenterDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<TagDto>> Handle(GetTags request, CancellationToken cancellationToken)
        {
            return await _dbContext.Tags
           .Select(k => new TagDto
           {
               TagName = k.Nazwa
           }).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
