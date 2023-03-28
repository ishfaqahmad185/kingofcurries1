using System.ComponentModel.DataAnnotations;

namespace KingofCurries.Models
{
	public class ContactUs
	{

	
		public int ContactUsId { get; set; }

		[Required]
		public string name { get; set; }
		[Required]
		public string email { get; set; }
		[Required]
		public string number { get; set; }
		[Required]
		public string subject { get; set; }
		[Required]
		public string message { get; set; }

		public ContactUs()
		{
			ContactUsId = 0;
			name = String.Empty;
			email = String.Empty;
			number = String.Empty;
			subject = String.Empty;
			message = String.Empty;

		}
	}
}
