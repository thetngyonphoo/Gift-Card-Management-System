﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Request
{
    public class UpdatePromoCodeRequest
    {
        public int Id { get; set; }        
        public string PhoneNumber { get; set; }
    }
}
