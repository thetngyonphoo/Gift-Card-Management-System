using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Data.Dtos.Response;
using GiftCardManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.IServices
{
    public interface ILoginService
    {
        int GetUserIdFromToken(ClaimsPrincipal user);
        Task<RegistrationResponse> Registration(RegistrationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshToken);
        Tokens GenerateJWTTokens(int userId,string userName);
        Task<UserRefreshToken> AddUserRefreshTokens(UserRefreshToken user);
        void DeleteUserRefreshTokens(string username, string refreshToken);
    }
}
