using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class GetPromoCodeResponse : ResponseStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string QRCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
