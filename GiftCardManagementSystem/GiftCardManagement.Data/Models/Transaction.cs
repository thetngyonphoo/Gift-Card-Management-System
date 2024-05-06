using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Models
{
    public class Transaction : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public virtual GiftCard GiftCard { get; set; }
        public int GiftCardId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual Payment Payment { get; set; }
        public int PaymentId { get; set; }              
        public DateTime PurchaseDate { get; set; }
        public Double AmountPaid { get; set; }
        public int CashbackStatus { get; set; }
    }
}
