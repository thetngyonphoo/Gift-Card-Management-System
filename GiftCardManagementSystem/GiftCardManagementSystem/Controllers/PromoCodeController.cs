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
    public class PromoCodeController : ControllerBase
    {
        private readonly ILogger<PromoCodeController> _logger;
        private readonly IPromoCodeService _iPromoCodeService;
        private readonly ILoginService _iLoginService;

        public PromoCodeController(IPromoCodeService promoCodeService, ILoginService loginService, ILogger<PromoCodeController> logger)
        {
            _iPromoCodeService = promoCodeService;
            _iLoginService = loginService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetPromoCode")]
        public async Task<IActionResult> GetPromoCode()
        {
            try
            {
                var response = await _iPromoCodeService.GetPromoCode();
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
        [Route("SavePromoCode")]
        public async Task<IActionResult> SavePromoCode(SavePromoCodeRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard SavePromoCode Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iPromoCodeService.SavePromoCode(request, currentLoginID);
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
        [Route("GetPromoCodeId")]
        public async Task<IActionResult> GetPromoCodeId([FromQuery] GetPromoCodeRequest request)
        {
            try
            {
                var response = await _iPromoCodeService.GetPromoCodeId(request);
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
        [Route("UpdatePromoCode")]
        public async Task<IActionResult> UpdatePromoCode(UpdatePromoCodeRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard UpdatePromoCode Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iPromoCodeService.UpdatePromoCode(request, currentLoginID);
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
        [Route("DeletePromoCode")]
        public async Task<IActionResult> DeletePromoCode(DeletePromoCodeRequest request)
        {
            try
            {
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iPromoCodeService.DeletePromoCode(request, currentLoginID);
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
