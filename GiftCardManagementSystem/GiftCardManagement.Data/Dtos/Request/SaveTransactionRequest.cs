using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Request
{
    public class SaveTransactionRequest
    {
        public int GiftCardId { get; set; }
        public int PaymentId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Double AmountPaid { get; set; }
        public int CashbackStatus { get; set; }
        /// <summary>       
        /// 1 => UsedCashback <br/>
        /// 2 => UnusedCashback <br/>
        /// </summary>
    }
}
