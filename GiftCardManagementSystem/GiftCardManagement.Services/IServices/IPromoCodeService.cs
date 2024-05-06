using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Data.Dtos.Response;
using GiftCardManagement.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.IServices
{
    public interface IPromoCodeService
    {
        Task<PromoCodeResponse> GetPromoCode();
        Task<ResponseStatus> SavePromoCode(SavePromoCodeRequest request, int currentLoginID);
        Task<GetPromoCodeResponse> GetPromoCodeId(GetPromoCodeRequest request);
        Task<ResponseStatus> UpdatePromoCode(UpdatePromoCodeRequest request, int currentLoginID);
        Task<ResponseStatus> DeletePromoCode(DeletePromoCodeRequest request, int currentLoginID);
    }
}
