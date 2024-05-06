using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Request
{
    public class DeleteGiftCardRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
