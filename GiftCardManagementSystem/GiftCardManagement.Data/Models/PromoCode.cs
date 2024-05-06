using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Models
{
    public class PromoCode : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string QRCode { get; set; }
        public string PhoneNumber { get; set; }
        
    }
}
