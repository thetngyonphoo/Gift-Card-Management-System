using GiftCardManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.IServices
{
    public interface IExcelExportService
    {
        IList<Customer> GetSalesReports();

        IList<Buyer> GetBuyerReports();
    }
}
