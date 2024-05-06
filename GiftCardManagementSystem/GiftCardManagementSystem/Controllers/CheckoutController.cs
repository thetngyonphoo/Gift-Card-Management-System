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
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly ICheckoutService _iCheckoutService;
        private readonly ILoginService _iLoginService;

        public CheckoutController(ICheckoutService checkoutService, ILoginService loginService, ILogger<CheckoutController> logger)
        {
            _iCheckoutService = checkoutService;
            _iLoginService = loginService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetCheckout")]
        public async Task<IActionResult> GetCheckout([FromQuery] GetCheckoutRequest request)
        {
            try
            {
                var response = await _iCheckoutService.GetCheckout(request);
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
        [Route("SaveTransaction")]
        public async Task<IActionResult> SaveTransaction(SaveTransactionRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard SaveTransaction Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iCheckoutService.SaveTransaction(request, currentLoginID);
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
        [Route("GetPurchaseHistory")]
        public async Task<IActionResult> GetPurchaseHistory()
        {
            try
            {
                var response = await _iCheckoutService.GetPurchaseHistory();
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
