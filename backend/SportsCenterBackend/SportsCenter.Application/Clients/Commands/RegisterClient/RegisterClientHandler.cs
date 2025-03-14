using MediatR;
using SportsCenter.Application.Exceptions;
using SportsCenter.Application.Exceptions.UsersException;
using SportsCenter.Application.Security;
using SportsCenter.Core.Entities;
using SportsCenter.Core.Repositories;

namespace SportsCenter.Application.Clients.Commands.RegisterClient;

internal sealed class RegisterClientHandler : IRequestHandler<RegisterClient, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IPasswordManager _passwordManager;

    public RegisterClientHandler(IUserRepository userRepository, IClientRepository clientRepository, IPasswordManager passwordManager)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        _passwordManager = passwordManager;
    }

    public async Task<Unit> Handle(RegisterClient request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
        if (existingUser != null) throw new UserAlreadyExistsException(request.Email);

        var securedPassword = _passwordManager.Secure(request.Password);

        var newOsoba = new Osoba
        {
            Imie = request.Name,
            Nazwisko = request.Surname,
            Adres = request.Address,
            DataUr = DateOnly.FromDateTime(request.BirthDate),
            Email = request.Email,
            Haslo = securedPassword,
            NrTel = request.PhoneNumber
        };

        var newClient = new Klient
        {
            KlientNavigation = newOsoba,
            Saldo = 0
        };

        await _clientRepository.AddClientAsync(newClient, cancellationToken);

        return Unit.Value;
    }
}