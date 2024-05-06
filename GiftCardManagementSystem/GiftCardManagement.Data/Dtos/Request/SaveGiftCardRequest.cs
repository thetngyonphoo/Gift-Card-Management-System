using GiftCardManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Request
{
    public class SaveGiftCardRequest
    {
        public string Title { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string GiftCardNo { get; set; }
        public Double Amount { get; set; }
        public int Quantity { get; set; }        
        public int TypeOfBuyingId { get; set; }
        public int PromoCodeId { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; }
    }
}
