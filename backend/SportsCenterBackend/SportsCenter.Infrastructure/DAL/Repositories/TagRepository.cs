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

    public async Task<bool> AddTagAsync(string tagName, CancellationToken cancellationToken)
    {
        bool tagExists = await _dbContext.Tags.AnyAsync(t => t.Nazwa == tagName, cancellationToken);
        if (tagExists)
        {
            return false;
        }

        var tag = new Tag { Nazwa = tagName };

        await _dbContext.Tags.AddAsync(tag, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
    public async Task<bool> RemoveTagAsync(int tagId, CancellationToken cancellationToken)
    {
        var tag = await _dbContext.Tags.FindAsync(new object[] { tagId }, cancellationToken);
        if (tag == null)
        {
            return false;
        }

        _dbContext.Tags.Remove(tag);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}