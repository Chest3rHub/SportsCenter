using SportsCenterBackend.DTOs;
using SportsCenterBackend.Entities;

namespace SportsCenterBackend.Services;

public interface IRegisterDbService
{
    Task RegisterClientAsync(RegisterClientDTO clientDto);

}