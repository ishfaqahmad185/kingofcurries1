using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UserAddress
    {
        public int UserAddressId { get; set; }
        public int UserId { get; set; }
        public string UserCountryNmae { get; set; }
        public string UserCountyName { get; set; }
        public string UserPostalCode { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string UserContactNumber { get; set; }
        public string Town { get; set; }
        public string zipCode { get; set; }
        public string state { get; set; }

        public int StatusId { get; set; }
        public int Remark { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

        public UserAddress()
        {
                
        }
    }
}
