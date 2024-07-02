using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Data.Models
{
    public class Customer
    {
        [DisplayNameAttribute("Sales Person Name")]
        public string SalesPerson { get; set; }
        [Bindable(false)]
        public string SalesJanJun { get; set; }
        public string SalesJulDec { get; set; }

        public Customer(string name, string janToJun, string julToDec)
        {
            SalesPerson = name;
            SalesJanJun = janToJun;
            SalesJulDec = julToDec;
        }
    }
}
