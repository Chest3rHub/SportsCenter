using SportsCenter.Core.Exceptions;

namespace SportsCenter.Application.Exceptions.UsersException;

public sealed class UserAlreadyExistsException : CustomException
{
    public string Email { get; set; }

    public UserAlreadyExistsException(string email) : base($"Email: {email} is already in use.")
    {
        Email = email;
    }
}