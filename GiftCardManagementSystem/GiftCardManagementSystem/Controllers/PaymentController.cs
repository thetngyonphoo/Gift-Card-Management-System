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
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _iPaymentService;
        private readonly ILoginService _iLoginService;

        public PaymentController(IPaymentService paymentService, ILoginService loginService, ILogger<PaymentController> logger)
        {
            _iPaymentService = paymentService;
            _iLoginService = loginService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetPaymentType")]
        public async Task<IActionResult> GetPaymentType()
        {
            try
            {
                var response = await _iPaymentService.GetPaymentType();
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
        [Route("SavePaymentType")]
        public async Task<IActionResult> SavePaymentType(SavePaymentTypeRequest request)
        {
            try
            {
                _logger.Log(LogLevel.Information, $"[ GiftCard SaveTypeOfBuying Request ] [{Request.Method} ] [{Request.Path}]{JsonConvert.SerializeObject(request)}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var currentLoginID = _iLoginService.GetUserIdFromToken(User);
                var response = await _iPaymentService.SavePaymentType(request, currentLoginID);
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
