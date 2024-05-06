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
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _iConfiguration;

        public CheckoutService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _iConfiguration = configuration;
        }
        public async Task<GetCheckoutResponse> GetCheckout(GetCheckoutRequest request)
        {
            GetCheckoutResponse response = new GetCheckoutResponse();

            var giftCard = await (from gf in _dbContext.GiftCard
                                  join ty in _dbContext.TypeOfBuying on gf.TypeOfBuyingId equals ty.Id
                                  join codes in _dbContext.PromoCode on gf.PromoCodeId equals codes.Id
                                  where gf.IsActive == true && ty.IsActive == true
                                  && gf.Id == request.GiftCardId
                                  select new
                                  {
                                      Id = gf.Id,
                                      Title = gf.Title,
                                      ExpiryDate = gf.ExpiryDate,
                                      GiftCardNo = gf.GiftCardNo,
                                      Amount = gf.Amount,
                                      Quantity = gf.Quantity,
                                      TypeOfBuyingId = gf.TypeOfBuyingId,
                                      TypeOfBuying = ConvertEnumToStringName<BuyingStatus>(ty.TypeOfBuyingStatus),
                                      PromoCode = codes.Code,
                                      Discount = gf.Discount,
                                      Description = gf.Description,
                                  }).FirstOrDefaultAsync();

            var payment = await _dbContext.Payment.Where(x => x.Id == request.PaymentId && x.IsActive == true).FirstOrDefaultAsync();

            if (giftCard != null && payment != null)
            {
                response.Title = giftCard.Title;
                response.ExpiryDate = giftCard.ExpiryDate;
                response.GiftCardNo = giftCard.GiftCardNo;
                response.Amount = giftCard.Amount;
                response.Quantity = giftCard.Quantity;
                response.TypeOfBuyingStatus = giftCard.TypeOfBuying;
                response.PromoCode = giftCard.PromoCode;
                response.Discount = giftCard.Discount;
                response.PaymentType = payment.PaymentType;
                response.PaymentDiscountPercentage = payment.DiscountPercentage == null ? 0 : payment.DiscountPercentage;

                var giftCardPrice = giftCard.Amount - (giftCard.Amount * (giftCard.Discount / 100));
                var giftCardQtyPrice = giftCardPrice * giftCard.Quantity;
                var totalPricePercentage = giftCardQtyPrice - response.PaymentDiscountPercentage;

                response.TotalAmount = totalPricePercentage;
                response.Description = giftCard.Description;
            }
            else
            {
                return new GetCheckoutResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "There is no data!"
                };
            }
            return response;
        }

        private static string ConvertEnumToStringName<T>(int testResult)
        {
            return Enum.GetName(typeof(T), testResult);
        }

        public async Task<ResponseStatus> SaveTransaction(SaveTransactionRequest request, int currentLoginID)
        {
            try
            {

                Transaction transaction = new Transaction();

                transaction.UserId = currentLoginID;
                transaction.GiftCardId = request.GiftCardId;
                transaction.PaymentId = request.PaymentId;
                transaction.AmountPaid = request.AmountPaid;
                transaction.PurchaseDate = request.PurchaseDate;
                transaction.CashbackStatus = request.CashbackStatus;
                transaction.CreatedBy = currentLoginID;
                transaction.CreatedDate = DateTime.Now;
                transaction.IsActive = true;
                await _dbContext.Transaction.AddAsync(transaction);
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

        public async Task<PurchaseHistoryResponse> GetPurchaseHistory()
        {
            PurchaseHistoryResponse responses = new PurchaseHistoryResponse();
            responses.PurchaseHistoryList = new List<PurchaseHistoryList>();

            var purchaseHistoryList = await (from tr in _dbContext.Transaction
                                             join user in _dbContext.User on tr.UserId equals user.Id
                                             join gf in _dbContext.GiftCard on tr.GiftCardId equals gf.Id
                                             join pay in _dbContext.Payment on tr.PaymentId equals pay.Id
                                             where gf.IsActive == true && user.IsActive == true
                                             && pay.IsActive == true && tr.IsActive == true
                                             select new
                                             {
                                                 Id = tr.Id,
                                                 Title = gf.Title,
                                                 ExpiryDate = gf.ExpiryDate,
                                                 GiftCardNo = gf.GiftCardNo,                                                
                                                 PromoCode = _dbContext.PromoCode.Where(x => x.Id == gf.PromoCodeId && x.IsActive == true).Select(x => x.Code).FirstOrDefault(),
                                                 UserName = user.Username,
                                                 PaymetType = pay.PaymentType,
                                                 PurchaseDate = tr.PurchaseDate,
                                                 AmountPaid = tr.AmountPaid,
                                                 CashbackStatus = ConvertEnumToStringName<CashbackStatus>(tr.CashbackStatus),
                                             }).ToListAsync();

            if (purchaseHistoryList != null && purchaseHistoryList.Count > 0)
            {
                foreach (var item in purchaseHistoryList)
                {
                    PurchaseHistoryList purchase = new PurchaseHistoryList();
                    purchase.GiftCardTitle = item.Title;
                    purchase.ExpiryDate = item.ExpiryDate;
                    purchase.GiftCardNo = item.GiftCardNo;
                    purchase.PromoCode = item.PromoCode;
                    purchase.AmountPaid = item.AmountPaid;
                    purchase.UserName = item.UserName;
                    purchase.PaymentType = item.PaymetType;
                    purchase.PurchaseDate = item.PurchaseDate;
                    purchase.CashbackStatus = item.CashbackStatus;
                    responses.PurchaseHistoryList.Add(purchase);
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
    }
}
