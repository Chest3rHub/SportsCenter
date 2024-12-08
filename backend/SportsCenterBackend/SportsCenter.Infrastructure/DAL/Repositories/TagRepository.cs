using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Infrastructure.DAL.Repositories;

public class TagRepository : ITagRepository
{
    private readonly SportsCenterDbContext _dbContext;

    public TagRepository(SportsCenterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds, CancellationToken cancellationToken)
    {
        return await _dbContext.Tags
            .Where(t => tagIds.Contains(t.TagId))
            .ToListAsync(cancellationToken);
    }
    
}