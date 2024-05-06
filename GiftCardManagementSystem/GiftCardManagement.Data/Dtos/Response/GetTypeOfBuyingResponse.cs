using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Dtos.Response
{
    public class GetTypeOfBuyingResponse : ResponseStatus
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string GiftForName { get; set; }
        public string GiftForPhoneNumber { get; set; }
        public string MaxLimitSelf { get; set; }
        public string MaxLimitOther { get; set; }
        public string TypeOfBuyingStatus { get; set; }
    }
}
