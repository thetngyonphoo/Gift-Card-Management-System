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
using System.Drawing;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _iConfiguration;

        public PromoCodeService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _iConfiguration = configuration;
        }

        public async Task<PromoCodeResponse> GetPromoCode()
        {
            PromoCodeResponse responses = new PromoCodeResponse();
            responses.PromoCodeList = new List<PromoCodeList>();

            var promoCodes = await _dbContext.PromoCode.Where(x => x.IsActive == true).ToListAsync();

            if (promoCodes != null && promoCodes.Count > 0)
            {
                foreach (var item in promoCodes)
                {
                    PromoCodeList codes = new PromoCodeList();
                    codes.Code = item.Code;
                    codes.QRCode = item.QRCode;
                    codes.PhoneNumber = item.PhoneNumber;

                    responses.PromoCodeList.Add(codes);
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

        public async Task<GetPromoCodeResponse> GetPromoCodeId(GetPromoCodeRequest request)
        {
            try
            {
                GetPromoCodeResponse response = new GetPromoCodeResponse();
                var promoCode = await _dbContext.PromoCode.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();

                if (promoCode != null)
                {
                    response.Id = promoCode.Id;
                    response.Code = promoCode.Code;
                    response.QRCode = promoCode.QRCode;
                    response.PhoneNumber = promoCode.PhoneNumber;
                    response.StatusCode = StatusCodes.Status200OK;
                }
                return response;
            }
            catch (Exception ex)
            {

                return new GetPromoCodeResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseStatus> SavePromoCode(SavePromoCodeRequest request, int currentLoginID)
        {
            try
            {
                PromoCode codes = new PromoCode();

                bool isValidCode = IsValidPromoCode(request.Code);
                if (!isValidCode)
                {
                    return new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Code!"
                    };
                }

                var isExistCode = await _dbContext.PromoCode.AnyAsync(x => x.IsActive == true && x.Code == request.Code);
                if (isExistCode)
                {
                    return new ResponseStatus
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Promo Code already exist!"
                    };
                }

                codes.Code = request.Code;
                codes.PhoneNumber = request.PhoneNumber;

                string qrCodeString = GenerateQRCode(request.Code);
                codes.QRCode = qrCodeString;

                codes.CreatedBy = currentLoginID;
                codes.CreatedDate = DateTime.Now;
                codes.IsActive = true;
                await _dbContext.PromoCode.AddAsync(codes);
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

        public bool IsValidPromoCode(string promoCode)
        {
            // Check if the promo code has the correct length
            if (promoCode.Length != 11)
            {
                return false;
            }

            // Check if the first 6 characters are digits
            for (int i = 0; i < 6; i++)
            {
                if (!char.IsDigit(promoCode[i]))
                {
                    return false;
                }
            }

            // Check if the next 5 characters are alphabets
            for (int i = 6; i < 11; i++)
            {
                if (!char.IsLetter(promoCode[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private string GenerateQRCode(string promoCode)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(promoCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Image qrCodeImage = qrCode.GetGraphic(20);

            // Convert image to byte array
            var bytes = ImageToByteArray(qrCodeImage);

            // Convert byte array to base64-encoded string
            var base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public async Task<ResponseStatus> UpdatePromoCode(UpdatePromoCodeRequest request, int currentLoginID)
        {
            try
            {

                var codes = await _dbContext.PromoCode.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();
                if (codes != null)
                {
                    codes.PhoneNumber = request.PhoneNumber;

                    string qrCodeString = GenerateQRCode(codes.Code);
                    codes.QRCode = qrCodeString;

                    codes.QRCode = "qrCodePath";
                    // string qrCodePath = GenerateQRCode(request.Code);

                    codes.UpdatedBy = currentLoginID;
                    codes.UpdatedDate = DateTime.Now;
                    codes.IsActive = true;
                    _dbContext.PromoCode.Update(codes);
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

        public async Task<ResponseStatus> DeletePromoCode(DeletePromoCodeRequest request, int currentLoginID)
        {
            var res = new ResponseStatus();

            var promoCode = await _dbContext.PromoCode.Where(x => x.Id == request.Id && x.IsActive == true).FirstOrDefaultAsync();
            if (promoCode == null)
            {
                res.StatusCode = StatusCodes.Status404NotFound;
                res.Message = "Request not found!";
            }
            else
            {
                promoCode.IsActive = false;
                promoCode.UpdatedBy = currentLoginID;
                promoCode.UpdatedDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                res.StatusCode = StatusCodes.Status200OK;
                res.Message = "Delete Successful!";
            }
            return res;
        }
    }
}
