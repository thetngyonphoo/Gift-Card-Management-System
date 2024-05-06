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
    public interface IGiftCardService
    {
        Task<GiftCardResponse> GetGiftCard();
        Task<ResponseStatus> SaveGiftCard(SaveGiftCardRequest request, int currentLoginID);
        Task<GetGiftCardResponse> GetGiftCardId(GetGiftCardRequest request);
        Task<ResponseStatus> UpdateGiftCard(UpdateGiftCardRequest request, int currentLoginID);
        Task<ResponseStatus> DeleteGiftCard(DeleteGiftCardRequest request, int currentLoginID);
    }
}
