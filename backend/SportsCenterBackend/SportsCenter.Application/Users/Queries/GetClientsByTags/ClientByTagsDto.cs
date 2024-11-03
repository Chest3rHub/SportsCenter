using SportsCenter.Core.Entities;

namespace SportsCenter.Application.Users.Queries.GetClientsByTags;

public class ClientByTagsDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<TagDto> Tags { get; set; }
}
