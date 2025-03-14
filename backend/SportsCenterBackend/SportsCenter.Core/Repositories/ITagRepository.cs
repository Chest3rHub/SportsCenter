using SportsCenter.Core.Entities;

namespace SportsCenter.Core.Repositories;
public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> tagIds, CancellationToken cancellationToken);
    Task<bool> AddTagAsync(string tagName, CancellationToken cancellationToken);
    Task<bool> RemoveTagAsync(int tagId, CancellationToken cancellationToken);
}