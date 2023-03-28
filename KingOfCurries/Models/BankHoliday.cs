using System.ComponentModel.DataAnnotations;

namespace KingofCurries.Models
{
	public class BankHoliday
	{
        public int BankHolidayId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }

        [Required(ErrorMessage = "BankHoliyday Date is Required")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM/dd/yyyy}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime Dates { get; set; }
        public bool Status { get; set; }

        public string SysTime { get; set; }



        public BankHoliday()
        {
            BankHolidayId = 0;
            UserId = 0;
            Title = String.Empty;
            Status = false;
            Dates = DateTime.MinValue;

        }
    }
    
}
