using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Models
{
    public class TypeOfBuying : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string GiftForName { get; set; }
        public string GiftForPhoneNumber { get; set; }
        public string MaxLimitSelf { get; set; }
        public string MaxLimitOther { get; set; }
        public int TypeOfBuyingStatus { get; set; }
    }
}
