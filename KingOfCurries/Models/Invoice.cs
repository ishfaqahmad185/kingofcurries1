using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int UserId { get; set; }
        public string MailingAddress { get; set; }
        public string CountryName { get; set; }
        public string CountyName { get; set; }
        public string PostalCode { get; set; }
        public string ContactNo { get; set; }
        public decimal TotalAmount { get; set; }
        public int StatusId { get; set; }
        public int AddressId { get; set; }
        public int PaymentId { get; set; }
        public string Remark { get; set; }
        public int TotalItems { get; set; }
        public decimal InvoiceDeliveyCharges { get; set; }
        public decimal InvoiceSubTotal { get; set; }

        public Invoice()
        {

        }
    }
}
