using KingofCurries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class GenericUser
    {
       
        public Users Users { get; set; }

        public UserAddress UserAddress { get; set; }=new UserAddress();
        public Address Address { get; set; }=new Address();
        public List<Invoice> Invoice { get; set; }=new List<Invoice>();
        public Invoice Invoices { get; set; } = new Invoice();

        public GenericUser()
        {
            UserAddress = new UserAddress();
        }


    }
}
