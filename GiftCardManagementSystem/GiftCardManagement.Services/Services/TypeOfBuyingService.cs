using Azure;
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
    public class TypeOfBuyingService : ITypeOfBuyingService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _iConfiguration;

        public TypeOfBuyingService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _iConfiguration = configuration;
        }

        public async Task<TypeOfBuyingResponse> GetTypeOfBuying()
        {
            TypeOfBuyingResponse responses = new TypeOfBuyingResponse();
            responses.TypeOfBuyingList = new List<TypeOfBuyingList>();

            var typeOfBuying = await (from tb in _dbContext.TypeOfBuying
                                      join user in _dbContext.User on tb.UserId equals user.Id
                                      where tb.IsActive == true && user.IsActive == true
                                      select new
                                      {
                                          Id = tb.Id,
                                          UserId = tb.UserId,
                                          UserName = user.Username,
                                          UserPhoneNumber = user.PhoneNumber,
                                          GiftForName = tb.GiftForName,
                                          GiftForPhoneNumber = tb.GiftForPhoneNumber,
                                          MaxLimitSelf = tb.MaxLimitSelf,
                                          MaxLimitOther = tb.MaxLimitOther,
                                          TypeOfBuyingStatus = ConvertEnumToStringName<BuyingStatus>(tb.TypeOfBuyingStatus)
                                      }).ToListAsync();

            if (typeOfBuying != null && typeOfBuying.Count > 0)
            {
                foreach (var item in typeOfBuying)
                {
                    TypeOfBuyingList type = new TypeOfBuyingList();
                    type.UserId = item.UserId;
                    type.UserName = item.UserName;
                    type.UserPhoneNumber = item.UserPhoneNumber;
                    type.GiftForName = item.GiftForName;
                    type.GiftForPhoneNumber = item.GiftForPhoneNumber;
                    type.MaxLimitSelf = item.MaxLimitSelf;
                    type.MaxLimitOther = item.MaxLimitOther;
                    type.TypeOfBuyingStatus = item.TypeOfBuyingStatus;
                    responses.TypeOfBuyingList.Add(type);
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

        public async Task<GetTypeOfBuyingResponse> GetTypeOfBuyingId(GetTypeOfBuyingRequest request)
        {
            try
            {
                GetTypeOfBuyingResponse response = new GetTypeOfBuyingResponse();
                var typeOfBuying = await _dbContext.TypeOfBuying.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();

                if (typeOfBuying != null)
                {
                    response.Id = typeOfBuying.Id;
                    response.UserId = typeOfBuying.UserId;
                    response.GiftForName = typeOfBuying.GiftForName;
                    response.GiftForPhoneNumber = typeOfBuying.GiftForPhoneNumber;
                    response.MaxLimitSelf = typeOfBuying.MaxLimitSelf;
                    response.MaxLimitOther = typeOfBuying.MaxLimitOther;
                    response.TypeOfBuyingStatus = ConvertEnumToStringName<BuyingStatus>(typeOfBuying.TypeOfBuyingStatus);

                    response.StatusCode = StatusCodes.Status200OK;
                }
                return response;
            }
            catch (Exception ex)
            {

                return new GetTypeOfBuyingResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseStatus> SaveTypeOfBuying(SaveTypeOfBuyingRequest request, int currentLoginID)
        {
            try
            {
                var res = new ResponseStatus();

                TypeOfBuying type = new TypeOfBuying();
                type.UserId = request.UserId;
                type.GiftForName = request.GiftForName;
                type.GiftForPhoneNumber = request.GiftForPhoneNumber;
                type.MaxLimitSelf = request.MaxLimitSelf;
                type.MaxLimitOther = request.MaxLimitOther;
                type.TypeOfBuyingStatus = request.TypeOfBuyingStatus;
                type.CreatedBy = currentLoginID;
                type.CreatedDate = DateTime.Now;
                type.IsActive = true;
                await _dbContext.TypeOfBuying.AddAsync(type);
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

        public async Task<ResponseStatus> UpdateTypeOfBuying(UpdateTypeOfBuyingRequest request, int currentLoginID)
        {
            try
            {
                var typeOfBuying = await _dbContext.TypeOfBuying.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();
                if (typeOfBuying != null)
                {
                    typeOfBuying.UserId = request.UserId;
                    typeOfBuying.GiftForName = request.GiftForName;
                    typeOfBuying.GiftForPhoneNumber = request.GiftForPhoneNumber;
                    typeOfBuying.MaxLimitSelf = request.MaxLimitSelf;
                    typeOfBuying.MaxLimitOther = request.MaxLimitOther;
                    typeOfBuying.TypeOfBuyingStatus = request.TypeOfBuyingStatus;
                    typeOfBuying.UpdatedBy = currentLoginID;
                    typeOfBuying.UpdatedDate = DateTime.Now;
                    typeOfBuying.IsActive = true;
                    _dbContext.TypeOfBuying.Update(typeOfBuying);
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

        public async Task<ResponseStatus> DeleteTypeOfBuying(DeleteTypeOfBuyingRequest request, int currentLoginID)
        {
            var res = new ResponseStatus();

            var typeOfBuying = await _dbContext.TypeOfBuying.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();
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
