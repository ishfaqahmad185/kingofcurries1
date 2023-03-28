namespace KingofCurries.Models
{
	public class CustomerReviews
	{

		public int ReviewId { get; set; } = 0;
		public int Rating { get; set; } = 0;
		public string ReviewName { get; set; } = string.Empty;
		public string ReviewEmail { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public DateTime CreatedOn { get; set; } = DateTime.MinValue;
		public string SysDate { get; set; } = string.Empty;

	}


	public class GenericCustomerReview
	{
		public List<CustomerReviews> CustomerReviews { get; set; }= new List<CustomerReviews>();
		public List<string> Images { get; set; }= new List<string>();
		public string FilesLocation { get; set; }= string.Empty;
	}
}
