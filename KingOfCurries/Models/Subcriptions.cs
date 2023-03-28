namespace KingofCurries.Models
{
	public class Subcriptions
	{
        public int SubcriptionsId { get; set; }=0;
        public string EmailAddress { get; set; }=String.Empty;
        public string type { get; set; } = String.Empty;
        public DateTime Date { get; set; }
        public Boolean Status { get; set; } = false;
    }
}
