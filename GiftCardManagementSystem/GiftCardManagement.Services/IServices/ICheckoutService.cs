using GiftCardManagement.Data.Dtos;
using GiftCardManagement.Data.Dtos.Request;
using GiftCardManagement.Data.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.IServices
{
    public interface ICheckoutService
    {
        Task<GetCheckoutResponse> GetCheckout(GetCheckoutRequest request);
        Task<PurchaseHistoryResponse> GetPurchaseHistory();
        Task<ResponseStatus> SaveTransaction(SaveTransactionRequest request, int currentLoginID);
    }
}
