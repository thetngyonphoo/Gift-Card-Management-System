using GiftCardManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class LoginResponse : ResponseStatus
    {
        public int UserId { get; set; }
        public Tokens Token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
