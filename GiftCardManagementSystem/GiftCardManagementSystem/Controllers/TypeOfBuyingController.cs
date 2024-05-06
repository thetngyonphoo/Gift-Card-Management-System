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
    public class TypeOfBuyingController : ControllerBase
    {
        private readonly ILogger<TypeOfBuyingController> _logger;
        private readonly ITypeOfBuyingService _iTypeOfBuyingService;
        private readonly ILoginService _iLoginService;

        public TypeOfBuyingController(ITypeOfBuyingService typeOfBuyingService, ILoginService loginService, ILogger<TypeOfBuyingController> logger)
        {
            _iTypeOfBuyingService = typeOfBuyingService;
            _iLoginService = loginService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetTypeOfBuying")]        
        public async Task<IActionResult> GetTypeOfBuying()
        {
            try
            {
                var response = await _iTypeOfBuyingService.GetTypeOfBuying();
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
        [Route("SaveTypeOfBuying")]
        public async Task<IActionResult> SaveTypeOfBuying(SaveTypeOfBuyingRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard SaveTypeOfBuying Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iTypeOfBuyingService.SaveTypeOfBuying(request, currentLoginID);
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
        [Route("GetTypeOfBuyingId")]
        public async Task<IActionResult> GetTypeOfBuyingId([FromQuery] GetTypeOfBuyingRequest request)
        {
            try
            {
                var response = await _iTypeOfBuyingService.GetTypeOfBuyingId(request);
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
        [Route("UpdateTypeOfBuying")]
        public async Task<IActionResult> UpdateTypeOfBuying(UpdateTypeOfBuyingRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard UpdateTypeOfBuying Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iTypeOfBuyingService.UpdateTypeOfBuying(request, currentLoginID);
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
        [Route("DeleteTypeOfBuying")]
        public async Task<IActionResult> DeleteTypeOfBuying(DeleteTypeOfBuyingRequest request)
        {
            try
            {
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iTypeOfBuyingService.DeleteTypeOfBuying(request, currentLoginID);
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
