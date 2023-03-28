using ExtensionMethods;
using KingofCurries.Models;
using Microsoft.AspNetCore.Mvc;
using KingofCurries.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Globalization;


using HarrysRestro.Services;
using Microsoft.Extensions.Hosting;
using System.Media;
using System.Data;

using Microsoft.Reporting.NETCore;

namespace KingofCurries.Controllers
{

	
	public class ClientsController : Controller
	{

		private readonly IHomeRepository _homeRepository;
		private readonly ISubscriptionsRepository _subscriptionsRepository;

		private readonly IEmailRepository _emailRepository;

		private readonly IMailService _mail;

		private readonly IOrderRepository _orderRepository;

		private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnvironment;

		private readonly IWebHostEnvironment _webHostEnvironment;

		private readonly IUserRepository _userRepository;


		public ClientsController(IHomeRepository homeRepository, ISubscriptionsRepository subscriptionsRepository, IEmailRepository emailRepository, IMailService mail, IOrderRepository orderRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnvironment, IWebHostEnvironment webHostEnvironment, IUserRepository userRepository)
		{
			_homeRepository = homeRepository;
			_subscriptionsRepository = subscriptionsRepository;
			_emailRepository = emailRepository;
			_mail = mail;
			_orderRepository = orderRepository;
			_subscriptionsRepository = subscriptionsRepository;
			_hostEnvironment = hostEnvironment;
			_webHostEnvironment = webHostEnvironment;
			_userRepository = userRepository;
		}



		public IActionResult NewReservations()
		{
			try
			{
                return View();
            }
			catch (Exception)
			{

				return View();
			}
		}



        public IActionResult Index()
		{
			ClientDashBoard clientDashBoard =new ClientDashBoard();
			try
			{
				if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
				{
					return RedirectToAction("Login", "Users");
				}
				else
				{
					if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
					{
						return RedirectToAction("Login", "Users");
					}
				}
				LoadOrderStatusDDL();
				DateTime FromDate = GetGMTDateTime().AddDays(-30);
				DateTime ToDate = GetGMTDateTime();
				ViewBag.GetDateRange = $"{FromDate.ToString("dd MMM, yyyy")} - {ToDate.ToString("dd MMM, yyyy")}";

				clientDashBoard= BindBashBoardData(FromDate, ToDate);
				return View(clientDashBoard);
				
			}
			catch (Exception)
			{

				return View(clientDashBoard);
			}
        
		}

		[HttpPost]
		public IActionResult Index(string daterange)
		{
			ClientDashBoard clientDashBoard = new ClientDashBoard();
			try
			{
				var array = daterange.Split('-');
				array[0] = array[0].Trim();
				array[1] = array[1].Trim();
				if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
				{
					return RedirectToAction("Login", "Users");
				}
				else
				{
					if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
					{
						return RedirectToAction("Login", "Users");
					}
				}
				DateTime FromDate = Convert.ToDateTime(array[0]);
				DateTime ToDate = Convert.ToDateTime(array[1]);
				clientDashBoard=BindBashBoardData(FromDate,ToDate);
				ViewBag.GetDateRange = daterange;
				return View(clientDashBoard);

			}
			catch (Exception)
			{

				return View(clientDashBoard);
			}
		
			
		}



		private ClientDashBoard BindBashBoardData(DateTime FromDate,DateTime ToDate)
		{
			var listData = _orderRepository.GetAllOrdersMain(-1).Where(x => x.DeliveryDate >= FromDate && x.OrderDate <= ToDate).ToList();
			var listDetailData = _orderRepository.GetAllOrderDetail(-1).ToList();
				listDetailData= listDetailData.Where(x => x.CreatedAt >= FromDate && x.CreatedAt <= ToDate).ToList();

			var listReservation = _homeRepository.GetAllReservation().Where(x => x.ReservationDate >= FromDate && x.ReservationDate <= ToDate).ToList();
			
			ClientDashBoard clientDashBoard = new ClientDashBoard();
			clientDashBoard.TotalOrders = listData.Count();
			clientDashBoard.DeliveryOrders = listData.Where(x => x.DeliveryType.ToLower() == "delivery").Count();
			clientDashBoard.CollectionOrders = listData.Where(x => x.DeliveryType.ToLower() == "collection").Count();
			clientDashBoard.TotalPayments = Math.Round(listData.Sum(x => x.GrandTotal), 2);
			clientDashBoard.DeliveryPayments = Math.Round(listData.Where(x => x.DeliveryType.ToLower() == "delivery").Sum(x => x.GrandTotal), 2);
			clientDashBoard.Collectionpayments = Math.Round(listData.Where(x => x.DeliveryType.ToLower() == "collection").Sum(x => x.GrandTotal), 2);
			clientDashBoard.DeliveryCharges = Math.Round(listData.Sum(x => x.DeliveryCharges), 2);
			clientDashBoard.Cardpayments = Math.Round(listData.Where(x => x.paymentType.ToLower() == "card").Sum(x => x.GrandTotal), 2);
			clientDashBoard.Cashpayments = Math.Round(listData.Where(x => x.paymentType.ToLower() == "cash").Sum(x => x.GrandTotal), 2);
			clientDashBoard.TotalCard = listData.Where(x => x.paymentType.ToLower() == "card").Count();
			clientDashBoard.TotalCash = listData.Where(x => x.paymentType.ToLower() == "cash").Count();
			clientDashBoard.NewOrders = listData.Where(x => x.StatusId != 6 || x.StatusId != 7).Count();
			clientDashBoard.CancelOrders = listData.Where(x => x.StatusId == 7).Count();
			clientDashBoard.TotalReservation=listReservation.Count();
			clientDashBoard.ConfrimReservation = listReservation.Where(x => x.Status == 2).Count();
			clientDashBoard.CancelReservation = listReservation.Where(x => x.Status == 3).Count();
			clientDashBoard.NewReservation = listReservation.Where(x => x.Status == 1).Count();
			clientDashBoard.TotalProducts = listDetailData.Count();
			var data = listData.GroupBy(x => x.Systime2).Select(e => new
			{
				date=e.Key,
				count=e.Count(),
			}).OrderBy(x => Convert.ToDateTime(x.date)).ToList();

			clientDashBoard.counts = data.Select(x => x.count).ToList();
			clientDashBoard.keys=data.Select(x=>Convert.ToDateTime(x.date).ToString("dd-MMM")).ToList();

			clientDashBoard.TopProducts = listDetailData.GroupBy(x => x.ItemName).Select(x =>
				new Top2Data()
				{
					Count = x.Count(),
					Key=x.Key,
				}).OrderByDescending(x=>x.Count).Take(3).ToList();

			clientDashBoard.TopLocation = listData.Where(x=>x.DeliveryId>0).GroupBy(x => x.DeliveryLocation).Select(x =>
				new Top2Data()
				{
					Count = x.Count(),
					Key = x.Key,
				}).OrderByDescending(x => x.Count).Take(3).ToList();
			return clientDashBoard;
		}


		public void LoadOrderStatusDDL()
		{


			IEnumerable<OrdersMain> listordersMains = _orderRepository.GetAllOrderStatus();
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			foreach (var item in listordersMains)
			{
				selectListItems.Add(new SelectListItem { Text = item.StatusName.ToString(), Value = item.StatusId.ToString() });
			}
			ViewBag.OrdersMain = selectListItems;

		}

		[HttpGet]
		public IActionResult AllOrders()
		{
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                {
                    return RedirectToAction("Login", "Users");
                }
            }
            LoadOrderStatusDDL();

		
			GenericModel gm = new GenericModel();
			gm= OrderList();

			return View(gm);
		}

		private GenericModel OrderList()
		{
			GenericModel genericModel = new GenericModel();
            int Statusid = 0;

			DateTime FromDate = GetGMTDateTime().AddMonths(-2);
			DateTime ToDate = GetGMTDateTime();

            genericModel.LOrder = _orderRepository.GetAllOrdersSearch("", "", "", FromDate, ToDate, Statusid).OrderByDescending(x=>x.OrderDate).ToList();


            genericModel.OrderSummary=BindViewBagData(genericModel.LOrder, FromDate, ToDate);

			return genericModel;
		}



		private OrderSummary BindViewBagData(List<OrdersMain> ListorderMains, DateTime FromDate, DateTime ToDate)
		{
			var totalPayment = Math.Round(ListorderMains.Sum(x => x.GrandTotal.ToDouble()), 2);
			var totalDeliveryCharges = Math.Round(ListorderMains.Where(x => x.DeliveryType == "delivery").Sum(x => x.DeliveryCharges.ToDouble()), 2);
			var collectionTotalPayment = Math.Round(ListorderMains.Where(x => x.DeliveryType == "collection").Sum(x => x.TotalAmount.ToDouble()), 2);
			var deliveryTotalPayment = Math.Round(ListorderMains.Where(x => x.DeliveryType != "collection").Sum(x => x.TotalAmount.ToDouble()), 2);
			var deliveryCounts = ListorderMains.Where(x => x.DeliveryType == "delivery").ToList().Count();
			var collectionCounts = ListorderMains.Where(x => x.DeliveryType == "collection").ToList().Count();

			var cashPayments = Math.Round(ListorderMains.Where(x => x.paymentType == "cash").Sum(x => x.GrandTotal.ToDouble()), 2);
			var cardPayments = Math.Round(ListorderMains.Where(x => x.paymentType == "card").Sum(x => x.GrandTotal.ToDouble()), 2);

			OrderSummary orderSummary = new OrderSummary();
			
			ViewBag.FromDate = FromDate.ToString("dd MMMM,yyyy");
			ViewBag.ToDate = ToDate.ToString("dd MMMM,yyyy");
            orderSummary.TotalPayment = totalPayment;
            orderSummary.DeliveryCharges = totalDeliveryCharges;
            orderSummary.TotalCollectionPayment = collectionTotalPayment;
            orderSummary.TotalDeliveryPayment = deliveryTotalPayment;
            orderSummary.TotalDeliveryOrders = deliveryCounts;
            orderSummary.TotalCollectionOrders = collectionCounts;
            orderSummary.CashPayment = cashPayments;
            orderSummary.CardPayment = cardPayments;

			return orderSummary;

		}


		[HttpPost]
		public IActionResult SearchOrders(string Username = "", string Invoicenumber = "", string ParcelType = "", string datepickerFromDate = "", string datepickerToDate = "", int StatusType = 0)
		{
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                {
                    return RedirectToAction("Login", "Users");
                }
            }
            if (Username == null)
			{
				Username = "";
			}

			if (Invoicenumber == null)
			{
				Invoicenumber = "";
			}
			if (ParcelType == null)
			{
				ParcelType = "";
			}

			if (StatusType == null)
			{
				StatusType = 0;
			}

			LoadOrderStatusDDL();
			GenericModel gm = new GenericModel();
			DateTime FromDate = Convert.ToDateTime(datepickerFromDate);
			DateTime ToDate = Convert.ToDateTime(datepickerToDate);
			var ListorderMains = _orderRepository.GetAllOrdersSearch(Username, Invoicenumber, ParcelType, FromDate, ToDate, StatusType).ToList();
			gm.LOrder = ListorderMains;
			ViewBag.Username = Username;
			ViewBag.Invoicenumber = Invoicenumber;
			ViewBag.ParcelType = ParcelType;
			ViewBag.FromDate = datepickerFromDate;
			ViewBag.ToDate = datepickerToDate;
			ViewBag.StatusType = StatusType;

			gm.OrderSummary=BindViewBagData(ListorderMains, FromDate, ToDate);

			return View("AllOrders", gm);
		}

		

		public IActionResult DoneOrders()
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
            {
                return RedirectToAction("Login", "Users");
            }
            else
            {
                if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                {
                    return RedirectToAction("Login", "Users");
                }
            }
            return View();
		}

		//[HttpGet]
		//public JsonResult AllOrderList()

		//{
		//    try
		//    {



		//        return Json(new { code = true, jsonText = "" });

		//    }
		//    catch (Exception exp)
		//    {

		//        return Json(new { code = false, jsonText = exp.Message.ToString() });
		//    }
		//}



		[HttpGet]
		[Route("order")]
		[Route("orders")]
		[Route("/Clients/TodayOrders")]
		[Route("/TodayOrders")]
		public IActionResult TodayOrders()
		{
			try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                    {
                        return RedirectToAction("Login", "Users");
                    }
                }
                return View();
			}
			catch (Exception)
			{

				return View();
			}
		}

		[HttpGet]

		public IActionResult GetContact()
		{
			try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                    {
                        return RedirectToAction("Login", "Users");
                    }
                }
                return View();
			}
			catch (Exception)
			{

				throw;
			}
		}


		[HttpGet]

		public IActionResult GetSubcriptions()
		{
			try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                    {
                        return RedirectToAction("Login", "Users");
                    }
                }
                return View();
			}
			catch (Exception)
			{

				throw;
			}
		}


		[HttpGet]

		public IActionResult GetPreparingOrders()
		{
			try
			{
				return View();
			}
			catch (Exception)
			{

				throw;
			}
		}

		[HttpGet]

		public IActionResult GetDeliveringOrders()
		{
			try
			{
				return View();
			}
			catch (Exception)
			{

				throw;
			}
		}

		[HttpGet]

		public IActionResult GetCollectionOrders()
		{
			try
			{
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                    {
                        return RedirectToAction("Login", "Users");
                    }
                }
                return View();
			}
			catch (Exception)
			{

				throw;
			}
		}


		[HttpGet]

		public IActionResult GetDoneOrders()
		{
			try
			{
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    return RedirectToAction("Login", "Users");
                }
                else
                {
                    if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                    {
                        return RedirectToAction("Login", "Users");
                    }
                }
                return View();
			}
			catch (Exception)
			{

				throw;
			}
		}


		public JsonResult ListOfContacts()
		{
			try
			{


				List<ContactUs> contactUs = _homeRepository.GetAllContacts().ToList();

				return Json(new { code = true, jsonText = contactUs });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}

		public JsonResult ListOfSubcriptions()
		{
			try
			{


				List<Subcriptions> subcriptions = _subscriptionsRepository.GetAllSucriptions().ToList();

				return Json(new { code = true, jsonText = subcriptions });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}

		public JsonResult DeleteContactUs(int id)
		{
			try
			{



				bool res = _homeRepository.DeleteContactUs(id);
				return Json(new { success = true, responseText = "Deleted Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}
		public JsonResult DeleteSubcriptions(int id)
		{
			try
			{



				bool res = _subscriptionsRepository.DeleteSubscriptions(id);
				return Json(new { success = true, responseText = "Deleted Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}




		public IActionResult AllReservations()
		{
			try
			{
				return View();
			}
			catch (Exception)
			{

				throw;
			}
		}
        public IActionResult CustomerByOrderCounts()
        {
            try
            {
				var listData=_userRepository.CustomersGetAllCustomerAndOrders();
                return View(listData);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult ListOfReservations()
		{
			try
			{


				List<Reservation> reservations = _homeRepository.GetAllReservation().ToList();

				return Json(new { code = true, jsonText = reservations });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}

		public IActionResult TodayReservations()
		{
			try
			{
				List<Reservation> reservations = _homeRepository.GetAllReservation().ToList().FindAll(x => x.SysDate == GetGMTDateTime().ToString("dd MMM, yyyy"));
				return View();
			}
			catch (Exception exp)
			{
				return View();
			}
		}

		public JsonResult ListOfTodayReservations()
		{
			try
			{


				List<Reservation> reservations = _homeRepository.GetTodayReservation().Where(x => x.ReservationDate.ToString("dd-MMM-yyyy") == GetGMTDateTime().ToString("dd-MMM-yyyy")).OrderByDescending(x => x.ReservationId).ToList();

				return Json(new { code = true, jsonText = reservations });
			}
			catch (Exception exp)
			{
				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}


        public JsonResult ListOfNewReservations()
        {
            try
            {


                List<Reservation> reservations = _homeRepository.GetTodayReservation().Where(x => x.Status==1).OrderByDescending(x=>x.ReservationId).ToList();

                return Json(new { code = true, jsonText = reservations });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        public JsonResult DeleteReservation(int id)
		{
			try
			{



				bool res = _homeRepository.DeleteReservation(id);
				return Json(new { success = true, responseText = "Deleted Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}


		public async Task<JsonResult> ConfirmReservation(int id)
		{
			try
			{

				Reservation reservation = _homeRepository.GetAllReservation().ToList().Where(x => x.ReservationId == id).FirstOrDefault();

				bool res = _homeRepository.ReservationChangeStatus(id, 2);





				EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(9);

				emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", reservation.UserName);
				emailTemplate.Subject = emailTemplate.Subject.Replace("#date#", reservation.ReservationDate.ToString("dd MMMM, yyyy"));
				emailTemplate.Subject = emailTemplate.Subject.Replace("#time#", reservation.ReservationTime);

				await _mail.SendMailAsync(reservation.Email, emailTemplate.MessageFor, emailTemplate.Subject);
				return Json(new { success = true, responseText = "Confirmed Successfully" });



			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}

		public async Task<JsonResult> RejectReservation(int id)
		{
			try
			{


				Reservation reservation = _homeRepository.GetAllReservation().ToList().Where(x => x.ReservationId == id).FirstOrDefault();

				bool res = _homeRepository.ReservationChangeStatus(id, 3);





				EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(10);

				emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", reservation.UserName);
				//emailTemplate.Subject = emailTemplate.Subject.Replace("#date#", reservation.ReservationDate.ToString("dd MMMM, yyyy"));
				//emailTemplate.Subject = emailTemplate.Subject.Replace("#time#", reservation.ReservationTime);

				await _mail.SendMailAsync(reservation.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				return Json(new { success = true, responseText = "Reject Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}

     

        public JsonResult OrdersMainAll()
		{
			try
			{


				List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == GetGMTDateTime().ToString("dd-MMM-yyyy")).ToList();
				orders = orders.Where(x => x.StatusId == 1 || x.StatusId == 2 || x.StatusId == 7).ToList();

				// List<OrdersMain> contactUs = _orderRepository.GetAllOrdersMain().Where(x => x.StatusId == 1 || x.StatusId == 2 || x.StatusId == 7).ToList();


				return Json(new { code = true, jsonText = orders });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}




		}
       
             public JsonResult GetCustomerDataByOrder()
        {
            try
            {


                List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == DateTime.Now.ToString("dd-MMM-yyyy")).ToList();
                orders = orders.Where(x => x.StatusId == 1 || x.StatusId == 2 || x.StatusId == 7).ToList();

                // List<OrdersMain> contactUs = _orderRepository.GetAllOrdersMain().Where(x => x.StatusId == 1 || x.StatusId == 2 || x.StatusId == 7).ToList();


                return Json(new { code = true, jsonText = orders });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }




        }
        public JsonResult OrdersPreparing()
		{
			try
			{
				List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == GetGMTDateTime().AddHours(-2).ToString("dd-MMM-yyyy") ).ToList();
				orders = orders.Where(c => c.StatusId == 3).ToList();


				//  List<OrdersMain> contactUs = _orderRepository.GetAllOrdersMain().Where(c => c.StatusId == 3).ToList();




				return Json(new { code = true, jsonText = orders });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}




		}

		public JsonResult OrdersDelivering()
		{
			try
			{


				List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == GetGMTDateTime().AddHours(-2).ToString("dd-MMM-yyyy")).ToList();
				orders = orders.Where(x => x.StatusId == 5 || x.StatusId == 4).ToList();

				//  List<OrdersMain> contactUs = _orderRepository.GetAllOrdersMain().Where(x =>  x.StatusId==5 || x.StatusId==4).ToList();


				return Json(new { code = true, jsonText = orders });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}




		}


		public JsonResult OrdersCollection()
		{
			try
			{


				List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == GetGMTDateTime().AddHours(-2).ToString("dd-MMM-yyyy")).ToList();
				orders = orders.Where(x => x.StatusId == 8).ToList();

				// List<OrdersMain> contactUs = _orderRepository.GetAllOrdersMain().Where(x => x.StatusId==8).ToList();


				return Json(new { code = true, jsonText = orders });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}




		}


		public JsonResult OrdersDone()
		{
			try
			{

                OrderSummary orderSummary = new OrderSummary();
                List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == GetGMTDateTime().AddHours(-2).ToString("dd-MMM-yyyy")).ToList();
				orders = orders.Where(x => x.StatusId == 6).ToList();

                // List<OrdersMain> contactUs = _orderRepository.GetAllOrdersMain().Where(x => x.StatusId == 6).ToList();
                if (orders.Count > 0)
                {
                    orderSummary = BindViewBagData(orders, GetGMTDateTime(), GetGMTDateTime());
                    return Json(new { code = true, jsonText = orders, summary= orderSummary });

                }

                return Json(new { code = false, jsonText = orders, summary = orderSummary });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}




		}

      
        [HttpPost]
		[Route("/ConfirmOrderStatusChange/{id}/{status}")]
		public async Task<JsonResult> ConfirmOrderStatusChange(int id, int status)
		{
			try
			{
				string emailTable = BindEmailOrderTable(id);
				var res = _orderRepository.OrdersConfirm(id, status);

				OrdersMain ordersMain = _orderRepository.GetAllOrdersMain(-1).ToList().Where(x => x.OrderId == id).FirstOrDefault();
				if (status == 2)
				{
					//accepted
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(12);
					
					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderId#", ordersMain.OrderNo.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#GrandTotal#", "€" + ordersMain.GrandTotal.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#Time#", ordersMain.PraperationDuration.ToString() + " minutes");

					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				}
				else if (status == 3)
				{
					//preparing
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(18);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);


					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				}
				else if (status == 4)
				{
					//ready
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(13);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);

					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				}
				else if (status == 5)
				{
					//out for delivery
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(14);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);

					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				}
				else if (status == 6)
				{
					//delivered
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(16);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);

					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				}
				else if (status == 7)
				{
					//cancel
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(17);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderId#", ordersMain.OrderNo.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#GrandTotal#", "€" + ordersMain.GrandTotal.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDate#", ordersMain.OrderDate.ToString("dd MMMM, yyyy"));
					//emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", ordersMain.OrderNo.ToString());
					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				}
				else if (status == 8)
				{
					//collection
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(15);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);


					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

				}
				else
				{
					//pending
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(11);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);
					//emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", ordersMain.UserName);
					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);


				}




				return Json(new { success = true, responseText = "Confirm Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}

		public JsonResult GetAllNewOrders()
		{
			try
			{
				if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserType"]))
				{

					var userType = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]).ToInt32();
					var isNewOrder = false;
					var orderMainData = new OrdersMain();

					if (userType == 2)
					{
						var check = _orderRepository.InvoiceMainGetNewOrder();
						if (check)
						{
							var webPath = _hostEnvironment.WebRootPath;
							var filepath = "audio\\new_job_order.wav";
							var completePath = Path.Combine(webPath, filepath);
							SoundPlayer simpleSound = new SoundPlayer(completePath);

							simpleSound.Play();

							isNewOrder = true;

							orderMainData = _orderRepository.GetAllOrdersMain(-1).ToList().FindAll(x => x.StatusId == 1).FirstOrDefault();



							return Json(new { success = true, responseText = "Send Successfully ", NewOrder = isNewOrder, orderMain = orderMainData, soundLocation = completePath });
						}
						else
						{
							return Json(new { success = false, responseText = "No New Order" });
						}


					}
					else
					{
						return Json(new { success = false, responseText = "Session Not Secure" });

					}


				}
				else
				{
					return Json(new { success = false, responseText = "Session Not Secure" });
				}
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}
		}


		[HttpPost]
		public JsonResult ChangeOrderduration(int orderId, int orderTime, int orderType)
		{
			try
			{
				var check = _orderRepository.ChangeOrderTime(orderId, orderTime, orderType);
				return Json(new { success = true, responseText = "Time Change Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}
		}

		[HttpPost]
		public async Task<JsonResult> NewOrderConfirmation(int orderId, int statusId, string duration)
		{
			try
			{
				string emailTable = BindEmailOrderTable(orderId);
				var ordersMain = _orderRepository.GetAllOrdersMain(orderId).FirstOrDefault();

				var check = _orderRepository.OrderMainOrderConfirmation(orderId, statusId, duration);
				if (statusId == 3)
				{
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(12);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderId#", ordersMain.OrderNo.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#GrandTotal#", "€" + ordersMain.GrandTotal.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#Time#", ordersMain.PraperationDuration.ToString() + " minutes");

					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

					 emailTemplate = _emailRepository.GetEmailTemplateId(18);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderId#", ordersMain.OrderNo.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#GrandTotal#", "€" + ordersMain.GrandTotal.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#Time#", ordersMain.PraperationDuration.ToString() + " minutes");

					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);

					return Json(new { success = true, responseText = "Order accepted Successfully" });
				}
				else
				{
					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(17);

					emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", ordersMain.UserName);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderId#", ordersMain.OrderNo.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);
					emailTemplate.Subject = emailTemplate.Subject.Replace("#GrandTotal#", "€" + ordersMain.GrandTotal.ToString());
					emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDate#", ordersMain.OrderDate.ToString("dd MMMM, yyyy"));
					//emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", ordersMain.OrderNo.ToString());
					await _mail.SendMailAsync(ordersMain.Email, emailTemplate.MessageFor, emailTemplate.Subject);
					return Json(new { success = true, responseText = "Order rejected Successfully" });
				}

			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}
		}





		[HttpPost]
		public JsonResult ChangeTimeData(int type, string dataId)
		{
			try
			{
				if (!string.IsNullOrEmpty(dataId))
				{
					string timeData = "";

					var array = dataId.Trim().Split(" ");
					DateTime dt = Convert.ToDateTime($"{GetGMTDateTime().ToString("dd-MMM-yyyy")} {array[1]}");

					if (type == 1)
					{
						dt = dt.AddMinutes(-10);
					}
					else
					{
						dt = dt.AddMinutes(10);
					}

					timeData = $"{array[0]} {dt.ToString("HH:mm")}";

					return Json(new { success = true, responseText = timeData });
				}
				else
				{
					throw new Exception("error occured");
				}


			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}
		}


		[HttpPost]
		public JsonResult GetOrderById(int id)
		{
			try
			{
				var data = _orderRepository.GetAllOrdersMain(id).FirstOrDefault();
				return Json(new { success = true, responseText = data });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}
		}


		
		public IActionResult ChangeOnlineOrder()
		{
			try
			{
				_homeRepository.ChangeOrderOnOff();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception exp)
			{
                return Json(new { success = false, responseText = exp.Message });
            }

		}

		public IActionResult PrintPreview()
		{
			return View();
		}

		private string BindEmailOrderTable(int OrderId)
		{
			string html = "";


			var orderMain = _orderRepository.GetAllOrdersMain(OrderId).FirstOrDefault();
			var data = _orderRepository.GetAllOrderDetail(OrderId).ToList();

			html += "<h4> Order Detail</h4>";
			html += "<table style=\"width:100%; border:1px solid black\">";
			html += "<thead>";
			html += "<tr style=\" border:1px solid black\">";
			html += "<td style=\" font-weight:bold\">S.No</td>";
			html += "<td style=\" font-weight:bold\">Item Name</td>";
			html += "<td style=\" font-weight:bold\">Qty.</td>";
			html += "<td style=\" font-weight:bold\">Price</td>";


			html += "</tr>";
			html += "</thead>";
			html += "<tbody>";
			int count = 1;
			foreach (var item in data)
			{
				html += "<tr style=\" border:1px solid black\">";
				html += "<td style=\" font-weight:bold\">" + count + "</td>";
				html += "<td style=\" font-weight:bold\">" + item.ItemName + "<br/>" + item.SubName + "<br/>" + item.FreeName + "</td>";
				html += "<td style=\" font-weight:bold\">" + item.Quantity + "</td>";
				html += "<td style=\" font-weight:bold\">" + item.TotalPrice + "</td>";


				html += "</tr>";
				count++;
			}
			html += "</tbody>";
			html += "<tbody>";
			html += "<tr style=\" border:1px solid black\">";
			html += "<td colspan=\"3\" style=\" font-weight:bold;text-align:right\">Sub Total:</td>";
			html += "<td style=\" font-weight:bold\">" + Math.Round(orderMain.TotalAmount, 2) + "</td>";
			html += "</tr>";
			html += "<tr style=\" border:1px solid black\">";
			html += "<td colspan=\"3\" style=\" font-weight:bold;text-align:right\">Delivery Total:</td>";
			html += "<td style=\" font-weight:bold\">" + Math.Round(orderMain.DeliveryCharges, 2) + "</td>";
			html += "</tr>";
			html += "<tr style=\" border:1px solid black\">";
			html += "<td colspan=\"3\" style=\" font-weight:bold;text-align:right\">Delivery Total:</td>";
			html += "<td style=\" font-weight:bold\">" + Math.Round(orderMain.GrandTotal, 2) + "</td>";
			html += "</tr>";
			html += "</tbody>";
			html += "</table>";



			return html;
		}


		private DateTime GetGMTDateTime()
		{
			int hour = _homeRepository.GetGMTTimeIncrement();

			DateTime MyTimeInWesternEurope2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Romance Standard Time").AddHours(hour);


			return MyTimeInWesternEurope2;


		}


	}
}
