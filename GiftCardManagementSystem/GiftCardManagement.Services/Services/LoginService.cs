using GiftCardManagement.Data.DBContext;
using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Data.Dtos.Response;
using GiftCardManagement.Data.Models;
using GiftCardManagement.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _iConfiguration;

        public LoginService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _iConfiguration = configuration;
        }
        public int GetUserIdFromToken(ClaimsPrincipal user)
        {
            int userId = 0;
            try
            {
                long loginedUserId = Int64.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
                userId = (int)loginedUserId;
            }
            catch (Exception ex)
            {

            }
            return userId;
        }
        public async Task<RegistrationResponse> Registration(RegistrationRequest request)
        {
            try
            {
                var isUserEmailexist = await _dbContext.User.AnyAsync(x => x.IsActive == true && x.Email == request.Email);
                if (isUserEmailexist == true)
                {
                    return new RegistrationResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "User Email has already registered!"
                    };
                }

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Description = request.Description,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedDate = DateTime.Now,                  
                    IsActive = true
                };
                await _dbContext.User.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                user.CreatedBy = user.Id;
                await _dbContext.SaveChangesAsync();

                var token = GenerateJWTTokens(user.Id,user.Username);

                UserRefreshToken obj = new UserRefreshToken
                {
                    RefreshToken = token.RefreshToken,
                    UserName = user.Username,
                    CreatedBy = user.Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                await _dbContext.UserRefreshToken.AddAsync(obj);
                await _dbContext.SaveChangesAsync();

                return new RegistrationResponse
                {
                    UserId = user.Id,
                    Tokens = token,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User Registration Success."
                };
            }
            catch (Exception ex)
            {
                return new RegistrationResponse
                {

                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };

            }
        }

        #region privateMethod
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
        #endregion
        public Tokens GenerateJWTTokens(int userId,string userName)
        {
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            string userIdString = userId.ToString();
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userIdString),                
                new Claim("UserId", userIdString),
                new Claim(ClaimTypes.Name, userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _iConfiguration.GetSection("JWT:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            int TokenExpireMinute = 10;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _iConfiguration.GetSection("JWT:ValidIssuer").Value,
                Audience = _iConfiguration.GetSection("JWT:ValidAudience").Value,
                Expires = now.AddMinutes(TokenExpireMinute),
                SigningCredentials = creds
            };

            var jwt = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(jwt);

            var refreshToken = GenerateRefreshToken();
            Tokens tokens = new()
            {
                AccessToken = token,
                RefreshToken = refreshToken,
            };

            return tokens;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var user = await _dbContext.User.Where(x => x.Email == request.Email && x.IsActive == true).FirstOrDefaultAsync();
                if (user is null)
                {
                    return new LoginResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Email!"
                    };
                }

                var isAuthorize = VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
                if (!isAuthorize)
                {
                    return new LoginResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Wrong Password"
                    };
                }

                return new LoginResponse
                {
                    UserId = user.Id,
                    Token = GenerateJWTTokens(user.Id, user.Username),
                    Name = user.Username,
                    Email = user.Email,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {

                return new LoginResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
            }
        }      

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_iConfiguration["JWT:SecretKey"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal;
            try
            {
                principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }

            return principal;
        }

        public async Task<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshToken)
        {
            return await _dbContext.UserRefreshToken.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken && x.IsActive == true);
        }

        public async Task<UserRefreshToken> AddUserRefreshTokens(UserRefreshToken user)
        {
            await _dbContext.UserRefreshToken.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async void DeleteUserRefreshTokens(string username, string refreshToken)
        {
            var item = await _dbContext.UserRefreshToken.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken);
            if (item != null)
            {
                _dbContext.UserRefreshToken.Remove(item);
            }
        }
    }
}
