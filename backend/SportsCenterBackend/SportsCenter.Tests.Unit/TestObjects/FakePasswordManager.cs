using SportsCenter.Application.Security;

namespace SportsCenter.Tests.Unit.TestObjects;

public class FakePasswordManager : IPasswordManager
{
    private HashSet<string> _correctPasswords = new();


    public FakePasswordManager()
    {
    }

    public FakePasswordManager(string password)
    {
        _correctPasswords.Add(password);
    }

    public string Secure(string password)
    {
        return password;
    }

    public bool Validate(string password, string securedPassword)
    {
        return _correctPasswords.Contains(password);
    }
}