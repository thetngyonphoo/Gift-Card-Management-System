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
    public class GiftCardService : IGiftCardService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _iConfiguration;

        public GiftCardService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _iConfiguration = configuration;
        }

        public async Task<GiftCardResponse> GetGiftCard()
        {
            GiftCardResponse responses = new GiftCardResponse();
            responses.GiftCardList = new List<GiftCardList>();

            var giftCardList = await (from gf in _dbContext.GiftCard
                                      join ty in _dbContext.TypeOfBuying on gf.TypeOfBuyingId equals ty.Id
                                      join codes in _dbContext.PromoCode on gf.PromoCodeId equals codes.Id
                                      where gf.IsActive == true && ty.IsActive == true
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
                                      }).ToListAsync();

            if (giftCardList != null && giftCardList.Count > 0)
            {
                foreach (var item in giftCardList)
                {
                    GiftCardList giftCard = new GiftCardList();
                    giftCard.Title = item.Title;
                    giftCard.ExpiryDate = item.ExpiryDate;
                    giftCard.GiftCardNo = item.GiftCardNo;
                    giftCard.Amount = item.Amount;
                    giftCard.Quantity = item.Quantity;
                    giftCard.TypeOfBuyingId = item.TypeOfBuyingId;
                    giftCard.TypeOfBuying = item.TypeOfBuying;
                    giftCard.PromoCode = item.PromoCode;
                    giftCard.Discount = item.Discount;
                    giftCard.Description = item.Description;
                    responses.GiftCardList.Add(giftCard);
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

        private static string ConvertEnumToStringName<T>(int testResult)
        {
            return Enum.GetName(typeof(T), testResult);
        }

        public async Task<GetGiftCardResponse> GetGiftCardId(GetGiftCardRequest request)
        {
            try
            {
                GetGiftCardResponse response = new GetGiftCardResponse();
                var giftCard = await (from gf in _dbContext.GiftCard
                                      join ty in _dbContext.TypeOfBuying on gf.TypeOfBuyingId equals ty.Id
                                      join codes in _dbContext.PromoCode on gf.PromoCodeId equals codes.Id
                                      where gf.IsActive == true && ty.IsActive == true
                                      && gf.Id == request.Id
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
                                          PromoCodeId = gf.PromoCodeId,
                                          PromoCode = codes.Code,
                                          Discount = gf.Discount,
                                          Description = gf.Description,
                                      }).FirstOrDefaultAsync();

                if (giftCard != null)
                {
                    response.Id = giftCard.Id;
                    response.Title = giftCard.Title;
                    response.ExpiryDate = giftCard.ExpiryDate;
                    response.GiftCardNo = giftCard.GiftCardNo;
                    response.Amount = giftCard.Amount;
                    response.Quantity = giftCard.Quantity;
                    response.Discount = giftCard.Discount;
                    response.Description = giftCard.Description;                   
                    response.TypeOfBuying = giftCard.TypeOfBuying;                   
                    response.PromoCode = giftCard.PromoCode;

                    response.StatusCode = StatusCodes.Status200OK;
                }
                return response;
            }
            catch (Exception ex)
            {

                return new GetGiftCardResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseStatus> SaveGiftCard(SaveGiftCardRequest request, int currentLoginID)
        {
            try
            {
                var isGiftCardNo = await _dbContext.GiftCard.AnyAsync(x => x.IsActive == true && x.GiftCardNo == request.GiftCardNo);
                if (isGiftCardNo)
                {
                    return new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Gift Card No already exist!"
                    };
                }

                GiftCard gf = new GiftCard();
                gf.Title = request.Title;
                gf.ExpiryDate = request.ExpiryDate;
                gf.GiftCardNo = request.GiftCardNo;
                gf.Amount = request.Amount;
                gf.Quantity = request.Quantity;
                gf.TypeOfBuyingId = request.TypeOfBuyingId;
                gf.PromoCodeId = request.PromoCodeId;
                gf.Discount = request.Discount;
                gf.Description = request.Description;
                gf.CreatedBy = currentLoginID;
                gf.CreatedDate = DateTime.Now;
                gf.IsActive = true;
                await _dbContext.GiftCard.AddAsync(gf);
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

        public async Task<ResponseStatus> UpdateGiftCard(UpdateGiftCardRequest request, int currentLoginID)
        {
            try
            {
                var giftCard = await _dbContext.GiftCard.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();

                if (giftCard != null)
                {
                    if (giftCard.GiftCardNo == request.GiftCardNo)
                    {
                        return new ResponseStatus
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Gift Card No already exist!"
                        };
                    }

                    giftCard.Title = request.Title;
                    giftCard.ExpiryDate = request.ExpiryDate;
                    giftCard.GiftCardNo = request.GiftCardNo;
                    giftCard.Amount = request.Amount;
                    giftCard.Quantity = request.Quantity;
                    giftCard.TypeOfBuyingId = request.TypeOfBuyingId;
                    giftCard.Discount = request.Discount;
                    giftCard.Description = request.Description;
                    giftCard.PromoCodeId = request.PromoCodeId;
                    giftCard.UpdatedBy = currentLoginID;
                    giftCard.UpdatedDate = DateTime.Now;
                    giftCard.IsActive = true;
                    _dbContext.GiftCard.Update(giftCard);
                    await _dbContext.SaveChangesAsync();
                }
                return new ResponseStatus
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Update Successful."
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

        public async Task<ResponseStatus> DeleteGiftCard(DeleteGiftCardRequest request, int currentLoginID)
        {
            var res = new ResponseStatus();

            var typeOfBuying = await _dbContext.GiftCard.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();
            if (typeOfBuying == null)
            {
                res.StatusCode = StatusCodes.Status404NotFound;
                res.Message = "Request not found!";
            }
            else
            {
                typeOfBuying.IsActive = false;
                typeOfBuying.UpdatedBy = currentLoginID;
                typeOfBuying.UpdatedDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                res.StatusCode = StatusCodes.Status200OK;
                res.Message = "Delete Successful!";
            }
            return res;
        }
    }
}
