namespace SportsCenter.Application.Clients.Queries.GetClientsByAge;

public class ClientByAgeDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
