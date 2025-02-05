namespace SportsCenter.Application.Exceptions.SportActivitiesException;

public sealed class SportActivityNotFoundException : Exception
{
    public int Id { get; set; }
    
    public SportActivityNotFoundException(int id) : base($"SportActivity with id: {id} not found")
    {
        Id = id;
    }
    
}