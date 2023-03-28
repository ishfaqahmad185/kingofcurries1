namespace KingofCurries.Models
{
	public class Address
	{
        public int Id { get; set; } = 0;
        public string Country { get; set; } = String.Empty;
        public string Town { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public string AddreessType { get; set; } = String.Empty;
        public string Addresss { get; set; } = String.Empty;
        public string PostalCode { get; set; } = String.Empty;
        public string ContactNo { get; set; } = String.Empty;
        public int UserId { get; set; } = 0;
    }
}
