using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Models
{
    public class GiftCard : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ExpiryDate { get; set; }
        [Required]
        public string GiftCardNo { get; set; }
        public Double Amount { get; set; }
        public int Quantity { get; set; }
        public virtual TypeOfBuying TypeOfBuying { get; set; }
        public int TypeOfBuyingId { get; set; }
        public virtual PromoCode PromoCode { get; set; }
        public int PromoCodeId { get; set; }
        public Double Discount { get; set; }        
        public string Description { get; set; }
    }
}
