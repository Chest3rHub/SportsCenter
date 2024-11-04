namespace SportsCenter.Application.Exceptions;

public class TagLimitException : Exception
{
    public int ClientId { get; }

    public TagLimitException(int clientId)
        : base($"Client with ID -> {clientId}  has the maximum number of tags")
    {
        ClientId = clientId;
    }
}