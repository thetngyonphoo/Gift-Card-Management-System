using GiftCardManagement.Data.DBContext;
using GiftCardManagement.Data.Dtos;
using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Data.Dtos.Response;
using GiftCardManagement.Data.Enum;
using GiftCardManagement.Data.Models;
using GiftCardManagement.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _iConfiguration;

        public PaymentService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _iConfiguration = configuration;
        }

        public async Task<PaymentTypeResponse> GetPaymentType()
        {
            PaymentTypeResponse responses = new PaymentTypeResponse();
            responses.PaymentTypeList = new List<PaymentTypeList>();

            var paymentTypes = await _dbContext.Payment.Where(x => x.IsActive == true).ToListAsync();

            if (paymentTypes != null && paymentTypes.Count > 0)
            {
                foreach (var item in paymentTypes)
                {
                    PaymentTypeList type = new PaymentTypeList();
                    type.PaymentType = item.PaymentType;
                    type.DiscountPercentage = item.DiscountPercentage;

                    responses.PaymentTypeList.Add(type);
                    responses.StatusCode = StatusCodes.Status200OK;
                }
            }
            else
            {
                responses.StatusCode = StatusCodes.Status200OK;
                responses.Message = "There is no data.";
                return responses;
            }
            return responses;
        }

        public async Task<ResponseStatus> SavePaymentType(SavePaymentTypeRequest request, int currentLoginID)
        {
            try
            {
                var res = new ResponseStatus();

                Payment pay = new Payment();
                pay.PaymentType = request.PaymentType;
                pay.DiscountPercentage = request.DiscountPercentage;

                pay.CreatedBy = currentLoginID;
                pay.CreatedDate = DateTime.Now;
                pay.IsActive = true;
                await _dbContext.Payment.AddAsync(pay);
                await _dbContext.SaveChangesAsync();

                return new ResponseStatus
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Create Successful."
                };
            }
            catch (Exception ex)
            {
                return new ResponseStatus
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
            }
        }
    }
}
