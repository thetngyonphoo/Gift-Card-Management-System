using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class PaymentTypeResponse : ResponseStatus
    {
        public List<PaymentTypeList> PaymentTypeList { get; set; }
    }

    public class PaymentTypeList
    {
        public string PaymentType { get; set; }
        public double DiscountPercentage { get; set; }
    }
}
