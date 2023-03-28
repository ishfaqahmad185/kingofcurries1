using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingofCurries.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public int UserType { get; set; }
        public string EmailAddress { get; set; }
        public Boolean StatusId { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

    
    }


    public class ClientLogin
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
