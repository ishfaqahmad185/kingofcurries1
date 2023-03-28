using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }
        public int InvoiceId { get; set; }
        public int CourierId { get; set; }
        public int ItemId { get; set; }
        public string TrakingNo { get; set; }
        public string TrackingDetail { get; set; }
        public int UserId { get; set; }
        public decimal CourierCharges { get; set; }
        public decimal SalePrice { get; set; }
        public int OfferId { get; set; }
        public int Quantity { get; set; }
        public int StatusId { get; set; }
        public decimal SubPrice { get; set; }
        public string ProductName { get; set; }

        public InvoiceDetail()
        {

        }
    }
}
