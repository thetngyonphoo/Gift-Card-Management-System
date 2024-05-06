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
    public interface IPaymentService
    {
        Task<PaymentTypeResponse> GetPaymentType();
        Task<ResponseStatus> SavePaymentType(SavePaymentTypeRequest request, int currentLoginID);
    }
}
