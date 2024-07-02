using GiftCardManagement.Data.Models;
using GiftCardManagement.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftCardManagement.Services.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public IList<Customer> GetSalesReports()
        {
            List<Customer> reports = new List<Customer>();
            reports.Add(new Customer("Andy Bernard", "45000", "58000"));
            reports.Add(new Customer("Jim Halpert", "34000", "65000"));
            reports.Add(new Customer("Karen Fillippelli", "75000", "64000"));
            reports.Add(new Customer("Phyllis Lapin", "56500", "33600"));
            reports.Add(new Customer("Stanley Hudson", "46500", "52000"));
            return reports;
        }


        public IList<Buyer> GetBuyerReports()
        {
            // Example data retrieval logic
            return new List<Buyer>
        {
            new Buyer { Id = 1, Name = "John Doe", Sales = 1000m },
            new Buyer { Id = 2, Name = "Jane Smith", Sales = 1500m },
            new Buyer { Id = 3, Name = "Sam Brown", Sales = 2000m }
        };
        }

    }
}
