using SportsCenter.Application.Abstractions;

namespace SportsCenter.Application.Users.Queries.GetUserAccountInfo;

public class GetUserAccountInfo : IQuery<UserAccountInfoDto>
{
    public int Offset { get; set; } = 0;
    public GetUserAccountInfo(int offSet)
    {
        Offset = offSet;
    }
}
