using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Customers
    {

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        public string CustomerEmailAddress { get; set; }
        public string CustomerPassword { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public int TotalOrderCount { get; set; }



        public string SysDate
        {
            get
            {
                return CreatedAt.ToString("dd MMM, yyyy");
            }
        }
    }
}
