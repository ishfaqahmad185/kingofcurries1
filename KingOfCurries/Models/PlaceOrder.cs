using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.ComponentModel.DataAnnotations;

namespace KingofCurries.Models
{
	public class PlaceOrder
	{
		public int Id { get; set; } = 0;

		public string DeliveryType { get; set; }=string.Empty;
		public string PaymentType { get; set; } = string.Empty;
		public int DeliveryId { get; set; } = 0;
		public int UserId { get; set; } = 0;
		public string AddressType { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string Country { get; set; } = string.Empty;
		public string County { get; set; } = string.Empty;
		public string Town { get; set; } = string.Empty;
		public string EIR { get; set; } = string.Empty;
		public string PhoneNo { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Notes { get; set; }=string.Empty;
		public string Address2 { get; set; }=string.Empty;
		public string DeliveryTime { get; set; }=string.Empty;
		public int PaymentId { get; set; }=0;
		public decimal DeliveryCharges { get; set; }=0;
		public int DeliveryChargesId { get; set; }=0;
		public decimal TotalAmount { get; set; }=0;
		
	}
}
