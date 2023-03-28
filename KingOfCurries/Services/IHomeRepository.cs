using KingofCurries.Models;

namespace Services
{
	public interface IHomeRepository
	{
		bool InsertContactUs(ContactUs contactUs);
        bool DeleteContactUs(int id);
        IEnumerable<ContactUs> GetAllContacts();

        bool InsertReservation(Reservation reservation);
		IEnumerable<Reservation> GetAllReservation();
		IEnumerable<Reservation> GetTodayReservation();

		bool DeleteReservation(int id);
		int PaymentAdd(string PayToken, string Email, decimal paidAmount, string Method, int UserId, int PaymentLogId, string StripePaymentId, string ReceiptUrl);
		int PaymentLogAdd(string PayUniqueToken, string StripeToken, string StripeEmail, string PaymentMethod);
		bool PaymentLogUpdate(string StripeResponse, string PaymentRemarks, int PaymentLogId, int Status, string UsrEmail);


		bool ReservationChangeStatus(int id, int status);

		OrderOnlineOnOff OrderOnlineOnOff();
		bool ChangeOrderOnOff();

		bool LogXXXInsert(string text);

		int GetGMTTimeIncrement();


		List<CustomerReviews> GetAllCustomerReviews();


		bool UpdateItemImage(string url,string name);


	}
}