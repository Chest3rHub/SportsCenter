using SportsCenterBackend.DTOs;
using SportsCenterBackend.Entities;

namespace SportsCenterBackend.Services;

public interface ILoginDbService
{
    Task<Osoba> LoginAsync(LoginDTO login);
}