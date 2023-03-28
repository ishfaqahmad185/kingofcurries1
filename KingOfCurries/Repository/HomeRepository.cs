using API.Utility;
using ExtensionMethods;
using KingofCurries.Models;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Asn1.Cmp;
 
using System;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Repository
{
	public class HomeRepository : IHomeRepository
	{

		public Boolean InsertContactUs(ContactUs contactUs)
		{

			SqlParameter[] param =
		{
				new SqlParameter("@UserName", contactUs.name),
				new SqlParameter("@EmailAddress", contactUs.email),
				new SqlParameter("@PhoneNo", contactUs.number),
				new SqlParameter("@ServiceType", contactUs.subject),
				new SqlParameter("@Message", contactUs.message),



				};

			return SqlHelper.ExecuteProcedureNonQuery("spContactus_Insert", param);

		}
        public IEnumerable<Reservation> GetTodayReservation()
        {
            List<Reservation> Balobj = new List<Reservation>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("spReservations_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
				Reservation reservation = new Reservation();
				reservation.ReservationId = dataRow["ReservationId"].ToInt32();
				reservation.UserName = dataRow["UserName"].ToString();
				reservation.ReservationDate = Convert.ToDateTime(dataRow["ReservationDate"]);
				reservation.ReservationTime = dataRow["ReservationTime"].ToString();
				reservation.PhoneNo = dataRow["PhoneNo"].ToString();
				reservation.Email = dataRow["Email"].ToString();
				reservation.TotalGuest = dataRow["TotalGuest"].ToInt32();
				reservation.Status = dataRow["Status"].ToInt32();
				reservation.Message = dataRow["Message"].ToString();
                reservation.CreatedAt = Convert.ToDateTime(dataRow["CreatedAt"]);






                Balobj.Add(reservation);
            }
            return Balobj;

            //return datatable;


        }

        public IEnumerable<Reservation> GetAllReservation()
        {
            List<Reservation> Balobj = new List<Reservation>();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("spReservations_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
				Reservation reservation = new Reservation();
				reservation.ReservationId = dataRow["ReservationId"].ToInt32();
				reservation.UserName = dataRow["UserName"].ToString();
				reservation.ReservationDate = Convert.ToDateTime(dataRow["ReservationDate"]);
				reservation.ReservationTime = dataRow["ReservationTime"].ToString();
				reservation.PhoneNo = dataRow["PhoneNo"].ToString();
				reservation.Email = dataRow["Email"].ToString();
				reservation.TotalGuest = dataRow["TotalGuest"].ToInt32();
				reservation.UserId = dataRow["UserId"].ToInt32();
				reservation.Status = dataRow["Status"].ToInt32();
				reservation.Message = dataRow["Message"].ToString();
                reservation.CreatedAt= Convert.ToDateTime(dataRow["CreatedAt"]);






				Balobj.Add(reservation);
            }
            return Balobj;

            //return datatable;


        }

        public IEnumerable<ContactUs> GetAllContacts()
        {
            List<ContactUs> Balobj = new List<ContactUs>();
            SqlParameter[] param =
             {


                };

			 
            DataTable datatable = SqlHelper.GetTableFromSP("sp_Contact_GetAll", param);

            foreach (DataRow dataRow in datatable.Rows)
            {
                ContactUs contactUs = new ContactUs();

                contactUs.ContactUsId = dataRow["ContactUsId"].ToInt32();
                contactUs.name = dataRow["UserName"].ToString();
                contactUs.number = dataRow["PhoneNo"].ToString();
                contactUs.email = dataRow["EmailAddress"].ToString();
                contactUs.subject = dataRow["ServiceType"].ToString();
                contactUs.message = dataRow["Message"].ToString();
               

                 
                Balobj.Add(contactUs);
            }
            return Balobj;

            //return datatable;


        }

        public Boolean DeleteContactUs(int id)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@ContactUsId",id),
          

       };


            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_ContactUs_Delete", param);




            return data;


        }
        public bool InsertReservation(Reservation reservation)
		{
            SqlParameter[] param =
        {
                new SqlParameter("@UserName", reservation.UserName),
                new SqlParameter("@PhoneNo", reservation.PhoneNo),
                new SqlParameter("@Email", reservation.Email),
                new SqlParameter("@ReservationDate", reservation.ReservationDate),
                new SqlParameter("@ReservationTime", reservation.ReservationTime),
                new SqlParameter("@TotalGuest", reservation.TotalGuest),
                new SqlParameter("@Message", reservation.Message),
                new SqlParameter("@UserId", reservation.UserId),
            
			



				};

			return SqlHelper.ExecuteProcedureNonQuery("spReservation_Insert", param);
		}

        public Boolean DeleteReservation(int id)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@ReservationId",id),
		  //new SqlParameter("@UserId",UserId),

	   };

            Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_Reservation_Delete", param);
            return data;


        }
		public Boolean ReservationChangeStatus(int id,int status)
		{
			DataTable datatable = new DataTable();
			SqlParameter[] param =
			 {
		  new SqlParameter("@ReservationId",id),
		  new SqlParameter("@Status",status),
		  //new SqlParameter("@UserId",UserId),

	   };

			Boolean data = SqlHelper.ExecuteProcedureNonQuery("spReservation_ChangeStatus", param);
			return data;


		}

		public Boolean RejectReservation(int id)
		{
			DataTable datatable = new DataTable();
			SqlParameter[] param =
			 {
		  new SqlParameter("@ReservationId",id),
		  //new SqlParameter("@UserId",UserId),

	   };

			Boolean data = SqlHelper.ExecuteProcedureNonQuery("sp_Reservation_Reject", param);
			return data;


		}

		public int PaymentAdd(string PayToken, string Email, decimal paidAmount, string Method, int UserId, int PaymentLogId, string StripePaymentId, string ReceiptUrl)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@PayToken",PayToken),
          new SqlParameter("@Email",Email),
          new SqlParameter("@paidAmount",paidAmount),
          new SqlParameter("@Method",Method),
          new SqlParameter("@usrId",UserId),
          new SqlParameter("@PayLogId",PaymentLogId),
          new SqlParameter("@strPayId",StripePaymentId),
          new SqlParameter("@payReceiptUrl",ReceiptUrl),
		  //new SqlParameter("@UserId",UserId),

	   };

            var data = SqlHelper.ExecuteProcedureReturnString("spPayment_add", param).ToInt32();
            return data;


        }

        public int PaymentLogAdd(string PayUniqueToken, string StripeToken, string StripeEmail, string PaymentMethod)
        {
            DataTable datatable = new DataTable();
            SqlParameter[] param =
             {
          new SqlParameter("@PayUniqueToken",PayUniqueToken),
          new SqlParameter("@StrToken",StripeToken),
         new SqlParameter("@StrEmail",StripeEmail),
         new SqlParameter("@PayMethod",PaymentMethod),
		  //new SqlParameter("@UserId",UserId),

	   };

            var data = SqlHelper.ExecuteProcedureReturnString("spPaymentlog_add", param).ToInt32();
            return data;


        }

        public bool PaymentLogUpdate(string StripeResponse, string PaymentRemarks, int PaymentLogId, int Status, string UsrEmail)
        {
          

            DataTable datatable = new DataTable();
            SqlParameter[] param =
            {
          new SqlParameter("@strResponse",StripeResponse),
          new SqlParameter("@PayStatus",Status),
         new SqlParameter("@PayLogId",PaymentLogId),
         new SqlParameter("@PayRemarks",PaymentRemarks),
         new SqlParameter("@UsrEmail",UsrEmail),
		  //new SqlParameter("@UserId",UserId),

	   };

            var data = SqlHelper.ExecuteProcedureNonQuery("spPaymentLog_Update", param);
            return data;

        }


        public OrderOnlineOnOff OrderOnlineOnOff()
        {
            OrderOnlineOnOff orderOnlineOnOff = new OrderOnlineOnOff();
            SqlParameter[] param =
             {


                };


            DataTable datatable = SqlHelper.GetTableFromSP("spOrderOnlineOnOff_Get", param);

            foreach (DataRow dataRow in datatable.Rows)
            {

                orderOnlineOnOff.OnlineOrderId = dataRow["OnlineOrderId"].ToInt32();
                orderOnlineOnOff.OrderText = dataRow["OrderText"].ToString();
                orderOnlineOnOff.IsOrderOnline = Convert.ToBoolean(dataRow["IsOrderOnline"]);
               





            }
            return orderOnlineOnOff;

            //return datatable;


        }

        public bool ChangeOrderOnOff()
        {
            SqlParameter[] param =
             {


                };


            return SqlHelper.ExecuteProcedureNonQuery("spOrderOnlineOnOff_Change", param);


        }
        
        public bool LogXXXInsert(string text)
        {
            SqlParameter[] param =
             {

				new SqlParameter("@Text",text),
				};


            return SqlHelper.ExecuteProcedureNonQuery("spLogXXX_Insert", param);


        }

        public int GetGMTTimeIncrement()
        {
            SqlParameter[] param =
			 {

				
				};


			return SqlHelper.ExecuteProcedureReturnString("spDayLightSaving_GetIncrementHour", param).ToInt32();

		}

		public List<CustomerReviews> GetAllCustomerReviews()
		{
			List<CustomerReviews> Balobj = new List<CustomerReviews>();
			SqlParameter[] param =
			 {


				};


			DataTable datatable = SqlHelper.GetTableFromSP("spCustomerReview_GetAll", param);

			foreach (DataRow dataRow in datatable.Rows)
			{
				CustomerReviews customerReviews = new CustomerReviews();
				customerReviews.ReviewId = dataRow["ReviewId"].ToInt32();
				customerReviews.Rating = dataRow["Rating"].ToInt32();
				customerReviews.ReviewName = dataRow["ReviewName"].ToString();
				customerReviews.ReviewEmail = dataRow["ReviewEmail"].ToString();
				customerReviews.Message = dataRow["Message"].ToString();
				customerReviews.CreatedOn = dataRow["CreatedAt"].ToDate();
				customerReviews.SysDate = customerReviews.CreatedOn.ToString("MMM dd, yyyy");
			






				Balobj.Add(customerReviews);
			}
			return Balobj;
		}

		public bool UpdateItemImage(string url, string name)
		{
			SqlParameter[] param =
		{
				new SqlParameter("@itemImage", url),
				new SqlParameter("@itemTitle", name),
				



				};

			return SqlHelper.ExecuteProcedureNonQuery("spItemImage_update", param);
		}
	}
}
