using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

    }
}
