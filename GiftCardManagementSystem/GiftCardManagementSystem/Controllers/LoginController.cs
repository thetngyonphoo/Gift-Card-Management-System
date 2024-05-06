using Azure;
using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Data.Models;
using GiftCardManagement.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace GiftCardManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _iLoginService;

        public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            _iLoginService = loginService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Registration")]       
        public async Task<IActionResult> Registration(RegistrationRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard UserRegistration Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                var response = await _iLoginService.Registration(request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard Login Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var response = await _iLoginService.Login(request);
                return StatusCode(response.StatusCode, response);

            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> Refresh(Tokens token)
        {
            var principal = _iLoginService.GetPrincipalFromExpiredToken(token.AccessToken);
            Claim userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");
            string userIdValue = userIdClaim?.Value;
            int userId = int.Parse(userIdValue);
          
            var username = principal.Identity?.Name;
           
            var savedRefreshToken = await _iLoginService.GetSavedRefreshTokens(username, token.RefreshToken);

            if (savedRefreshToken.RefreshToken != token.RefreshToken)
            {
                return Unauthorized("Invalid attempt!");
            }

            var newJwtToken = _iLoginService.GenerateJWTTokens(userId,username);

            if (newJwtToken == null)
            {
                return Unauthorized("Invalid attempt!");
            }

            UserRefreshToken obj = new UserRefreshToken
            {
                RefreshToken = newJwtToken.RefreshToken,
                UserName = username
            };

            _iLoginService.DeleteUserRefreshTokens(username, token.RefreshToken);
            await _iLoginService.AddUserRefreshTokens(obj);

            return Ok(newJwtToken);
        }
    }
}

