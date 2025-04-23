namespace SportsCenter.Application.Clients.Queries.GetClientsByAge;

public class ClientByAgeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
}
