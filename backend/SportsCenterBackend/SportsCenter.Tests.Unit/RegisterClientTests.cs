using SportsCenter.Application.Clients.Commands.RegisterClient;
using SportsCenter.Application.Exceptions.UsersException;
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

        var fakeUserRepository = new FakeUserRepository(new List<Osoba>
    {
        new Osoba { Email = "john.doe@example.com" } 
    });

        var fakePasswordManager = new FakePasswordManager();

        var handler = new RegisterClientHandler(
            fakeUserRepository, 
            null,              
            fakePasswordManager 
        );
       
        // Act & Assert
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() =>
            handler.Handle(request, CancellationToken.None));
    }
}