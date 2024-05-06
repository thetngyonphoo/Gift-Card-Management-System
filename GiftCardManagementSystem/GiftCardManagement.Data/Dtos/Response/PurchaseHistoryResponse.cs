using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class PurchaseHistoryResponse : ResponseStatus
    {

        public List<PurchaseHistoryList> PurchaseHistoryList { get; set; }
    }

    public class PurchaseHistoryList
    {
        public string GiftCardTitle { get; set; }
        public string GiftCardNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserName { get; set; }
        public string PaymentType { get; set; }
        public string PromoCode { get; set; }        
        public DateTime PurchaseDate { get; set; }
        public Double AmountPaid { get; set; }
        public string CashbackStatus { get; set; }
    }
}
