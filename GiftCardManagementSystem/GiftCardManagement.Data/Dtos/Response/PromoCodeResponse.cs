using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class PromoCodeResponse : ResponseStatus
    {
        public List<PromoCodeList> PromoCodeList { get; set; }
    }

    public class PromoCodeList
    {
        public string Code { get; set; }
        public string QRCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
