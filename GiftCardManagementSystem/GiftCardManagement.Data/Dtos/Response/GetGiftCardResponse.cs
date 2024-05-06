using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class GetGiftCardResponse :ResponseStatus
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string GiftCardNo { get; set; }
        public Double Amount { get; set; }
        public int Quantity { get; set; }
        public int TypeOfBuyingId { get; set; }
        public string TypeOfBuying { get; set; }
        public int PromoCodeId { get; set; }
        public string PromoCode { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; }
    }
}
