using GiftCardManagement.Data.Dtos;
using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Data.Dtos.Response;
using GiftCardManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.IServices
{
    public interface ITypeOfBuyingService
    {
        Task<TypeOfBuyingResponse> GetTypeOfBuying();
        Task<ResponseStatus> SaveTypeOfBuying(SaveTypeOfBuyingRequest request, int currentLoginID);
        Task<GetTypeOfBuyingResponse> GetTypeOfBuyingId(GetTypeOfBuyingRequest request);
        Task<ResponseStatus> UpdateTypeOfBuying(UpdateTypeOfBuyingRequest request,int currentLoginID);
        Task<ResponseStatus> DeleteTypeOfBuying(DeleteTypeOfBuyingRequest request, int currentLoginID);
    }
}
