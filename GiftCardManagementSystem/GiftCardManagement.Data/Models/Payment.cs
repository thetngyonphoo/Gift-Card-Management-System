using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Models
{
    public class Payment : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public double DiscountPercentage { get; set; }
    }
}
