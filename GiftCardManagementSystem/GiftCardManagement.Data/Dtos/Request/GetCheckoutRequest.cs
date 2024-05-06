using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Request
{
    public class GetCheckoutRequest
    {
        public int GiftCardId { get; set; }
        public int PaymentId { get; set; }
    }
}
