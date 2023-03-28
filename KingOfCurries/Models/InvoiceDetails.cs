using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class InvoiceDetails
    {
        public int InvoiceDetailId { get; set; }
       
        public string TrackingNo { get; set; }
        public string TrackingDetail { get; set; }
        public string CourierCharges { get; set; }
        public Decimal SalePrice { get; set; }
        public int StatusId { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

    }
}
