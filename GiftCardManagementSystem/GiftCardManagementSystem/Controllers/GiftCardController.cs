using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Services.IServices;
using GiftCardManagement.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GiftCardManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftCardController : ControllerBase
    {
        private readonly ILogger<GiftCardController> _logger;
        private readonly IGiftCardService _iGiftCardService;
        private readonly ILoginService _iLoginService;

        public GiftCardController(IGiftCardService giftCardService, ILoginService loginService, ILogger<GiftCardController> logger)
        {
            _iGiftCardService = giftCardService;
            _iLoginService = loginService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetGiftCard")]
        public async Task<IActionResult> GetGiftCard()
        {
            try
            {
                var response = await _iGiftCardService.GetGiftCard();
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("SaveGiftCard")]
        public async Task<IActionResult> SaveGiftCard(SaveGiftCardRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard SaveGiftCard Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iGiftCardService.SaveGiftCard(request, currentLoginID);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetGiftCardId")]
        public async Task<IActionResult> GetGiftCardId([FromQuery] GetGiftCardRequest request)
        {
            try
            {
                var response = await _iGiftCardService.GetGiftCardId(request);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateGiftCard")]
        public async Task<IActionResult> UpdateGiftCard(UpdateGiftCardRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard UpdateGiftCard Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iGiftCardService.UpdateGiftCard(request, currentLoginID);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteGiftCard")]
        public async Task<IActionResult> DeleteGiftCard(DeleteGiftCardRequest request)
        {
            try
            {
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iGiftCardService.DeleteGiftCard(request, currentLoginID);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
