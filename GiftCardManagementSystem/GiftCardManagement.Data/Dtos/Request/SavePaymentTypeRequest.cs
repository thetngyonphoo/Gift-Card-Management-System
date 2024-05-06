using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Request
{
    public class SavePaymentTypeRequest
    {
        public string PaymentType { get; set; }
        public double DiscountPercentage { get; set; }
    }
}
