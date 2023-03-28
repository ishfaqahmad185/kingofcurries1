using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace KingofCurries.Models
{
	public class Reservation
	{
		public int? ReservationId { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		
		public string PhoneNo { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
		public DateTime ReservationDate { get; set; }
		[Required]

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
		public DateTime CreatedAt { get; set; }
		[Required]

		public string ReservationTime { get; set; }
		[Required]
		[IntegerValidator]
		public int TotalGuest { get; set; }
		[Required]
		public string Message { get; set; }

		public string? SysDateCreatedAt
		{
			get
			{
				return CreatedAt.ToString("dd MMM, yyyy");
			}
		}
		public string? SysDate { get {
				return ReservationDate.ToString("dd MMM, yyyy");
			} }
		public int UserId { get; set; }
		public int Status { get; set; }

		public Reservation()
		{
			ReservationId = 0;
			UserName =string.Empty;
			PhoneNo = string.Empty;
			Email = string.Empty;
			ReservationDate= DateTime.Now;
			CreatedAt = DateTime.Now;
			ReservationTime = string.Empty;
			TotalGuest = 0;
			Message = string.Empty;
			Status= 0;
		
		}
	}
}
