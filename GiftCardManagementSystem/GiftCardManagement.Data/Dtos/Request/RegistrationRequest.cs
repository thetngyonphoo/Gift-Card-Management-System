using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Request
{
    public class RegistrationRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }
    }
}
