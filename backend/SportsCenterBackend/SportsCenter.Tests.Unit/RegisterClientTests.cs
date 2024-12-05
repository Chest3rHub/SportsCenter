using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Users.Commands.RegisterClient;
using SportsCenter.Core.Entities;
using SportsCenter.Tests.Unit.TestObjects;

namespace SportsCenter.Tests.Unit;

public class RegisterClientTests
{
    [Fact]
    public async Task Handle_ShouldThrowUserAlreadyExistsException_WhenUserAlreadyExists()
    {
        // Arrange
        var request = new RegisterClient("John", "Doe", "john.doe@example.com", "password", DateTime.Now, "123456789",
            "123 Main St");
        var handler = new RegisterClientHandler(new FakeUserRepository(new List<Osoba>
        {
            new()
            {
                Email = "john.doe@example.com"
            }
        }), new FakePasswordManager());

        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() =>
            handler.Handle(request, CancellationToken.None));
    }
}