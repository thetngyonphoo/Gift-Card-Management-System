using GiftCardManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class RegistrationResponse :ResponseStatus
    {
        public int UserId { get; set; }
        public Tokens Tokens { get; set; }
    }
}
