using Ihospital.API.DTOs;

namespace Ihospital.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(int staffId, string userName);
    }
}
