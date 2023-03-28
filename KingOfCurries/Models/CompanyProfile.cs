namespace Models
{
    public class CompanyProfile
    {
        public int CompanyID { get; set; }
        public string CompanyTitle { get; set; }
        public IFormFile CompanyImage { get; set; }
        public string CompanyLogo { get; set; }
        public string Companyaddress { get; set; }
        public string CompanyPhoneNo { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyDetail { get; set; }
        public string CompanyShortname { get; set; }
        public int UserId { get; set; }

        public CompanyProfile()

        {
            CompanyID = 0;
            CompanyTitle = string.Empty;
            CompanyLogo = string.Empty;
            Companyaddress = string.Empty;
            CompanyPhoneNo = string.Empty;
            CompanyEmail = string.Empty;
            CompanyDetail = string.Empty;
            CompanyShortname = string.Empty;
            UserId= 0;

        }
    }
}
