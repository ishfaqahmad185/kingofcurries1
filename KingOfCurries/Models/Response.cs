namespace KingofCurries.Models
{
	public class Response
	{
		public Boolean IsSuccess { get; set; } = false;
		public int SuccessCode { get; set; } = 0;
		public string text { get; set; }=string.Empty;
	}
}
