using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    public class MySettingsModel
    {
        public string APIDbConnection { get; set; }
        public string DbConnection { get; set; }
        public string ClientName { get; set; }
        public Boolean IsLogHelperMethod { get; set; }
        public Boolean IsLogMainMethod { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string SMTPPort { get; set; }
    }
}
