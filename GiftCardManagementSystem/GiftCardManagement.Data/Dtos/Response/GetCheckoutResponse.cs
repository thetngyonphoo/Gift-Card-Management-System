using GiftCardManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class GetCheckoutResponse : ResponseStatus
    {
        public string Title { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string GiftCardNo { get; set; }
        public Double Amount { get; set; }
        public int Quantity { get; set; }             
        public string TypeOfBuyingStatus { get; set; }
        public string PromoCode { get; set; }
        public double Discount { get; set; }
        public string Description { get; set; }
        public string PaymentType { get; set; }
        public double PaymentDiscountPercentage { get; set; }
        public double TotalAmount { get; set; }

    }
}
