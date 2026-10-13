using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.DTOs.Response;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Services.Contracts;

public interface IUserService
{
    Task<AuthResponseDTO> AddUserAsync(RegisterRequestDTO request);
    Task<AuthResponseDTO> Login(LoginRequestDTO request);
    Task<AuthResponseDTO> RefreshToken(string email);
    Task<User> GetUserInfo(string email);
}
