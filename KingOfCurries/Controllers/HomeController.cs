using _Helper;
using ExtensionMethods;
using KingofCurries.Models;
using HarrysRestro.Repository;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using Stripe;
using HarrysRestro.Services;
using Org.BouncyCastle.Ocsp;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Media;
using Org.BouncyCastle.Utilities;
using Microsoft.CodeAnalysis;
using Models;
using System.Web;
using System.IO;

namespace KingofCurries.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly IMailService _mail;
        private readonly IEmailRepository _emailRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IItemRepository _itemRepository;
        private readonly ISubItemsRepository _subItems;
        private readonly IFreeItemsRepository _freeItems;
        private readonly ICustomerDataAccessLayer _customerDataAccess;
        private readonly IUserRepository _userRepository;
        private readonly IDeliveryChargesRepository _deliveryChargesRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IBankHolidayRepository _bankHolidayRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository, IMailService mail, IEmailRepository emailRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IItemRepository itemRepository, ISubItemsRepository subItems, IFreeItemsRepository freeItems, ICustomerDataAccessLayer customerDataAccess, IUserRepository userRepository, IDeliveryChargesRepository deliveryChargesRepository, IOrderRepository orderRepository, IBankHolidayRepository bankHolidayRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
            _mail = mail;
            _emailRepository = emailRepository;
            _hostingEnvironment = hostingEnvironment;
            _itemRepository = itemRepository;
            _subItems = subItems;
            _freeItems = freeItems;
            _customerDataAccess = customerDataAccess;
            _userRepository = userRepository;
            _deliveryChargesRepository = deliveryChargesRepository;
            _orderRepository = orderRepository;
            _bankHolidayRepository = bankHolidayRepository;
        }

        public IActionResult Index()
		{

			GenericCustomerReview genericCustomerReview = new GenericCustomerReview();
			
            var data = _homeRepository.GetAllCustomerReviews();

			if (data.Count > 0)
			{
				genericCustomerReview.CustomerReviews = data.OrderBy(arg => Guid.NewGuid()).Take(10).OrderBy(x => x.Rating).ToList();
			}
			else
			{
				genericCustomerReview.CustomerReviews = new List<CustomerReviews>();

			}

			string wwwRootPath = _hostingEnvironment.WebRootPath;
			string UploadedFolder = $"/images/Reviews/";
			var basePath = Path.Combine(wwwRootPath + UploadedFolder);
			var fileCount = (from file in Directory.EnumerateFiles(@$"{basePath}", "*.jpg", SearchOption.AllDirectories)
							 select file).Count();



			DirectoryInfo d = new DirectoryInfo(@$"{basePath}");
			FileInfo[] Files = d.GetFiles("*.png"); //Getting Text files


			foreach (FileInfo file in Files)
			{
				genericCustomerReview.Images.Add(
					 UploadedFolder + file.Name
				);
			}

			genericCustomerReview.FilesLocation = basePath;

            //	string userAgent = Request.Headers["User-Agent"].ToString();
            //    GetGMTDateTime();

            //	Boolean isMobileDevice = CheckDeviceType.CheckDevice(userAgent);

            //if (isMobileDevice)
            //{
            //    return RedirectToAction("Takeaway", "Menu");
            //}
            //else
            //{
            //    return View(genericCustomerReview);
            //}

            return View(genericCustomerReview);
        }


   //     public IActionResult Home()
   //     {
			//GenericCustomerReview genericCustomerReview = new GenericCustomerReview();
			//var data = _homeRepository.GetAllCustomerReviews();

			//if (data.Count > 0)
			//{
			//	genericCustomerReview.CustomerReviews = data.OrderBy(arg => Guid.NewGuid()).Take(10).OrderBy(x => x.Rating).ToList();
			//}
			//else
			//{
			//	genericCustomerReview.CustomerReviews = new List<CustomerReviews>();

			//}

			//string wwwRootPath = _hostingEnvironment.WebRootPath;
			//string UploadedFolder = $"/images/Reviews/";
			//var basePath = Path.Combine(wwwRootPath + UploadedFolder);
			//var fileCount = (from file in Directory.EnumerateFiles(@$"{basePath}", "*.jpg", SearchOption.AllDirectories)
			//				 select file).Count();



			//DirectoryInfo d = new DirectoryInfo(@$"{basePath}");
			//FileInfo[] Files = d.GetFiles("*.png"); //Getting Text files


			//foreach (FileInfo file in Files)
			//{
			//	genericCustomerReview.Images.Add(
			//		 UploadedFolder + file.Name
			//	);
			//}

			//genericCustomerReview.FilesLocation = basePath;
			//return View("Index",genericCustomerReview);
   //     }

        public IActionResult ResturantClosed()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("ContactUs")]
        public IActionResult ContactUs()
        {
            try
            {
                return View(new ContactUs());
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
  [Route("ContactUs")]
       public async Task<IActionResult> ContactUsAsync(ContactUs contactUs)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    ViewBag.successMsg = "Message Send Successfully";
                 _homeRepository.InsertContactUs(contactUs); 
                   
                
                    EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(5);

                    emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", contactUs.name);


                    await _mail.SendMailAsync(contactUs.email, emailTemplate.MessageFor, emailTemplate.Subject);
                    contactUs = new ContactUs();
                    ModelState.Clear();
                    return View(contactUs);
                }
				else
				{
                    ViewBag.errorMsg ="Invalid Data Kindly provide a valid data";
                    return View(contactUs);
                }
                
     
                
			}
			catch (Exception exp)
			{
                ViewBag.errorMsg=exp.Message;
                return View(contactUs);
				
			}

		}

        [HttpGet]
        [Route("FoodTest")]
        public IActionResult FoodTest()
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
        [Route("Catering")]
        public IActionResult Catering()
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
        [Route("Voucher")]
        public IActionResult Voucher()
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
        [Route("/ReserveTable")]
        public IActionResult ReserveTable()
        {
            try
            {

                Reservation reservation=new Reservation();
                reservation.ReservationDate = GetGMTDateTime();
                return View(reservation);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("ReserveTable")]
        public async Task<IActionResult> ReserveTableAsync(Reservation reservation)
        {
            try
            {
                reservation.ReservationTime = reservation.ReservationTime.Split(" ").LastOrDefault();
				bool CustomerName = (reservation.UserName != HttpUtility.HtmlEncode(reservation.UserName));
				bool PhoneNo = (reservation.PhoneNo != HttpUtility.HtmlEncode(reservation.PhoneNo));
				bool Message = (reservation.Message != HttpUtility.HtmlEncode(reservation.Message));

				if (CustomerName)
				{
					throw new Exception("UserName provided is invalid");
				}else if(PhoneNo)
                {
                    throw new Exception("Phone provided is invalid");
                }
                else if(Message)
                {
					throw new Exception("Message provided is invalid");
				}
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                    {

                        Users user = _userRepository.UserGetByEmail(reservation.Email);

                        if (user.UserId > 0)
                        {
                            reservation.UserId = user.UserId;
                            GetReservation(reservation);
                            reservation = new Reservation();
                            ModelState.Clear();
                            return View(new Reservation());
                        }
                        else
                        {
                            ForgetPassword forgetPassword = new ForgetPassword();
                            Customers customers = new Customers();
                            customers.CustomerName = reservation.UserName;
                            customers.CustomerEmailAddress = reservation.Email;
                            customers.CustomerPassword = forgetPassword.GeneratePassword();
                            customers.CustomerPhone=reservation.PhoneNo;
                            int customerId = _customerDataAccess.AddCustomer(customers);
                            reservation.UserId = customerId;
                            EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(2);
                            emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", reservation.UserName);
                            emailTemplate.Subject = emailTemplate.Subject.Replace("#Email#", reservation.Email);
                            emailTemplate.Subject = emailTemplate.Subject.Replace("#UserPassword#", customers.CustomerPassword);

                            await _mail.SendMailAsync(reservation.Email, emailTemplate.MessageFor, emailTemplate.Subject);


                            var userdata = _userRepository.GetUsersById(customerId);

                            await AuthUserAsync(userdata, true);


                            await GetReservation(reservation);
                            reservation = new Reservation();
                            ModelState.Clear();
                            return View(new Reservation());



                        }




                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            reservation.UserId = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"].ToString()).ToInt32();

                            await GetReservation(reservation);
                            reservation = new Reservation();
                            ModelState.Clear();
                            return View(new Reservation());
                        }
                        else
                        {
                            ViewBag.errorMsg = "Invalid Data Kindly provide a valid data";
                            return View(reservation);
                        }
                    }
                }
                else
                {
					string errors = JsonConvert.SerializeObject(ModelState.Values
  .SelectMany(state => state.Errors)
  .Select(error => error.ErrorMessage));
					ViewBag.errorMsg = errors;
					return View(reservation);
				}

			}
			catch (Exception exp)
            {
                ViewBag.errorMsg = exp.Message;
                return View(reservation);

            }
        }

		private async Task<bool> GetReservation(Reservation reservation)
		{


			DateTime dt = DateTime.ParseExact(reservation.ReservationTime, "HH:mm", null, DateTimeStyles.None);
			reservation.ReservationTime = dt.ToString("hh:mm tt");
		
			_homeRepository.InsertReservation(reservation);
            ViewBag.successMsg = "Table Reserved Successfully";

            EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(8);

			emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", reservation.UserName);
			emailTemplate.Subject = emailTemplate.Subject.Replace("#date#", reservation.ReservationDate.ToString("dd MMMM, yyyy"));
			emailTemplate.Subject = emailTemplate.Subject.Replace("#time#", reservation.ReservationTime);

			await _mail.SendMailAsync(reservation.Email, emailTemplate.MessageFor, emailTemplate.Subject);

			return true;

		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        [Route("Error404")]
        public IActionResult Error404()
		{
			try
			{
                return View();
            }
			catch (Exception exp)
			{

                ViewBag.errorMsg = exp.Message;
                return View();
            }
		} 
        
        [HttpGet]
        [Route("Error500")]
        public IActionResult Error500()
		{
			try
			{
                return View();
            }
			catch (Exception exp)
			{

                ViewBag.errorMsg = exp.Message;
                return View();
            }
		}

        [HttpGet]
        [Route("Gallery")]
        public IActionResult Gallery()
        {
            try
            {
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string UploadedFolder = $"/images/gallery/";
                var basePath = Path.Combine(wwwRootPath + UploadedFolder);
                var fileCount = (from file in Directory.EnumerateFiles(@$"{basePath}", "*.jpg", SearchOption.AllDirectories)
                                 select file).Count();
                List<Gallery> galleryList = new List<Gallery>();

                
                DirectoryInfo d = new DirectoryInfo(@$"{basePath}");
                FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files
             

                foreach (FileInfo file in Files)
                {
                    galleryList.Add(new Gallery
                    {
                        ImageUrl = UploadedFolder + file.Name
                    });
                }
                

                return View(galleryList);


            }
            catch (Exception exp)
            {

                ViewBag.errorMsg = exp.Message;
                return View();
            }

        }

        


        [HttpPost]
        public JsonResult AddToCartItem(int id, int quantity,bool isSubItem,bool isFreeItem,int subItemId,int freeItemId)
        {
            try
            {
              var check = _freeItems.GetAllFreeItems().ToList().FindAll(x => x.ItemId == id).Any(x => x.FreeItemId == freeItemId);

				if (isSubItem)
                {
                    var list = _subItems.GetAllSubItems().ToList().FindAll(x => x.ItemId == id);
                    if (list.Any(x=>x.SubItemId==subItemId))
                    {

                    }
                    else
                    {
                        throw new Exception("Invalid Sub Item");
                    }
                }
                if (isFreeItem)
                {
                    if (check)
                    {

                    }
                    else
                    {
						throw new Exception("Invalid Free Item");
					}
                }
                CookieOptions cookieOptionsNew = new CookieOptions();
                cookieOptionsNew.Expires = new DateTimeOffset(DateTime.Now.AddMonths(6));
                cookieOptionsNew.HttpOnly = true;
                cookieOptionsNew.Secure = true;
               

                CartModel model=new CartModel();
               string cookieeData  = "";

                var dataList = HttpContext.Request.Cookies.Keys.ToList();

                 foreach(var item in dataList)
                {
                    if (item.Contains("StoreCart"))
                    {
                        cookieeData += HttpContext.Request.Cookies[item];
                    }
                }

                string StoreCart = Protection.Decrypt(cookieeData);

              

                var data=JsonConvert.DeserializeObject<CartModel>(StoreCart);

                if (data != null)
                {
					var singleItem = _itemRepository.GetAllItemsById(id);
					CartProducts cartProducts=new CartProducts();
                    cartProducts.SubProductId = subItemId;
                    cartProducts.ProductId = id;
                    cartProducts.FreeProductId= freeItemId;
                    cartProducts.Quantity= quantity;
                    if (subItemId > 0)
                    {
                        cartProducts.Price=Math.Round(_subItems.GetAllSubItemsById(subItemId).SubItemPrice,2).ToDecimal();
                    }
                    else
                    {
                        cartProducts.Price = singleItem.Price.ToDecimal();
                    }
                   
                    cartProducts.UniqueId = Guid.NewGuid().ToString();

                    if (data.productIds.Any(x=>x.ProductId==id && x.SubProductId==subItemId && x.FreeProductId==freeItemId))
                    {
                        return Json(new { success = false, responseText = "Item is already in your cart" });
                    }
                    else
                    {
                        data.productIds.Add(cartProducts);
                      

                        var jsonObject = JsonConvert.SerializeObject(data);

                        string ProductId = Protection.Encrypt(jsonObject);

                        var arrayString2 = ChunksUpto(ProductId, 3500);

                        if (arrayString2.Count() == 1)
                        {


                            HttpContext.Response.Cookies.Append("StoreCart", ProductId, cookieOptionsNew);

                        }
                        else
                        {
                            var count = 0;

                           foreach(var item in arrayString2)
                            {
                                if (count == 0)
                                {
                                    HttpContext.Response.Cookies.Append("StoreCart", item, cookieOptionsNew);
                                    count++;
                                }
                                else
                                {
                                    HttpContext.Response.Cookies.Append($"StoreCart{count}", item, cookieOptionsNew);
                                    count++;
                                }
                            }
                        }

                        
                        return Json(new { success = true, responseText = singleItem });
                    }


                }
                else
                {
					var singleItem = _itemRepository.GetAllItemsById(id);
					data = new CartModel();
                    decimal totalPrice = 0;
					if (subItemId > 0)
					{
						totalPrice = Math.Round(_subItems.GetAllSubItemsById(subItemId).SubItemPrice, 2).ToDecimal();
					}
					else
					{
						totalPrice = singleItem.Price.ToDecimal();
					}
					data.productIds.Add(new CartProducts()
                    {
                        ProductId= id,
                        FreeProductId= freeItemId,
                        SubProductId= subItemId,
                        Quantity= quantity,
                        Price= totalPrice,
						UniqueId = Guid.NewGuid().ToString(),

				});
                    
                    var jsonObject = JsonConvert.SerializeObject(data);

                    string ProductId = Protection.Encrypt(jsonObject);


                    HttpContext.Response.Cookies.Append("StoreCart", ProductId, cookieOptionsNew);
                
                    
                    return Json(new { success = true, responseText = singleItem });
                }
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }



        }

        [HttpGet]
        public JsonResult ListCartDataJson()
        {

            try
            {

                string cookieeData = "";

                var dataList = HttpContext.Request.Cookies.Keys.ToList();

                foreach (var item in dataList)
                {
                    if (item.Contains("StoreCart"))
                    {
                        cookieeData += HttpContext.Request.Cookies[item];
                    }
                }

                string StoreCart = Protection.Decrypt(cookieeData);
                var data = new CartModel();

				if (!string.IsNullOrEmpty(StoreCart))
                {
					data = JsonConvert.DeserializeObject<CartModel>(StoreCart);
				}
             

                
                if (data!=null)
                {

                    List<ListCart> ListCart = new List<ListCart>();

                    foreach(var ele in data.productIds)
                    {
						ListCart listCart = new ListCart();
						listCart.Items = _itemRepository.GetAllItemsById(ele.ProductId);
                        listCart.Items.Quantity = ele.Quantity;
                        listCart.ProductId=ele.ProductId;
                        listCart.Quantity=ele.Quantity;
                        listCart.ProductSlug = ele.UniqueId;



						if (ele.SubProductId > 0)
                        {
							listCart.SubItemId = ele.SubProductId;
							listCart.SubItems=_subItems.GetAllSubItems().Where(x=>x.SubItemId==ele.SubProductId).FirstOrDefault();
                            listCart.SubItems.Quantity = listCart.Quantity;

						}
                        else
                        {
							listCart.SubItemId= 0;

						}
						if (ele.FreeProductId > 0)
						{
							listCart.FreeItemId = ele.FreeProductId;
							listCart.FreeItems = _freeItems.GetAllFreeItems().Where(x => x.FreeItemId == ele.FreeProductId).FirstOrDefault();
						}
						else
						{
							listCart.FreeItemId = 0;

						}

						ListCart.Add(listCart);
                    }

                  



                    var totalPrice = Math.Round(ListCart.Where(x=>x.SubItemId <= 0).Sum(x => x.Items.TotalPrice.ToDouble()), 2)+ Math.Round(ListCart.Where(x => x.SubItemId > 0).Sum(x => x.SubItems.TotalPrice.ToDouble()), 2);

					data.TotalPrice =totalPrice.ToDecimal();

					return Json(new { isSuccess=true,isData = true, responseText = ListCart,subPrice=totalPrice,deliveryCharges=data.DeliveryCharges });


                }
                else
                {
                    return Json(new { isSuccess = true,isData = false, responseText="error Occured" });
                }
            }
            catch (Exception exp)
            {


                return Json(new { success = false, responseText = exp.Message });
            }
        }


        


        [HttpPost]
        public JsonResult RemoveItemFromCart(int id)
        {
            try
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMonths(6));
                cookieOptions.HttpOnly = true;

                string cookieeData = "";

                var dataList = HttpContext.Request.Cookies.Keys.ToList();

                foreach (var item in dataList)
                {
                    if (item.Contains("StoreCart"))
                    {
                        cookieeData += HttpContext.Request.Cookies[item];
                    }
                }

                string StoreCart = Protection.Decrypt(cookieeData);

                var data = JsonConvert.DeserializeObject<CartModel>(StoreCart);


                if (data != null)
                {
                    var remData = data.productIds.FirstOrDefault(x => x.ProductId == id);
                    data.productIds.Remove(remData);


                    string jsonData = JsonConvert.SerializeObject(data);
                    string ProductIdList = Protection.Encrypt(jsonData);
                    var arrayString2 = ChunksUpto(ProductIdList, 3500);

                    if (arrayString2.Count() == 1)
                    {
                        int count = 0;

                        HttpContext.Response.Cookies.Append("StoreCart", ProductIdList, cookieOptions);

						foreach (var item in dataList)
						{
							if (item.Contains("StoreCart"))
							{

                                if (count == 0)
                                {
                                    count++;
                                }
                                else
                                {
									CookieOptions cookieOptionsNew = new CookieOptions();
									cookieOptionsNew.Expires = new DateTimeOffset(DateTime.Now.AddDays(-2));
									cookieOptionsNew.HttpOnly = true;
									HttpContext.Response.Cookies.Append($"StoreCart{count}", item, cookieOptionsNew);
									count++;
								}
								
							}
						}

					}
                    else
                    {
                        var count = 0;

                        foreach (var item in arrayString2)
                        {
                            if (count == 0)
                            {
                                HttpContext.Response.Cookies.Append("StoreCart", item, cookieOptions);
                                count++;
                            }
                            else
                            {
								
								HttpContext.Response.Cookies.Append($"StoreCart{count}", item, cookieOptions);
                                count++;
                            }
                        }
                    }

                    var products = _itemRepository.GetAllItemsById(id);
                    return Json(new { success = true, responseText = products });
                }
                else
                {
                    return Json(new { success = false, responseText = "No Item in Cart list" });
                }
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }



        }
        
        [HttpPost]
        public JsonResult UpdateItemFromCart(string productSlug,int quantity)
        {
            try
            {
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMonths(6));
                cookieOptions.HttpOnly = true;

                string cookieeData = "";

                var dataList = HttpContext.Request.Cookies.Keys.ToList();

                foreach (var item in dataList)
                {
                    if (item.Contains("StoreCart"))
                    {
                        cookieeData += HttpContext.Request.Cookies[item];
                    }
                }

                string StoreCart = Protection.Decrypt(cookieeData);

                var data = JsonConvert.DeserializeObject<CartModel>(StoreCart);


                if (data != null)
                {
                  data.productIds.FirstOrDefault(x => x.UniqueId == productSlug).Quantity=quantity;
            
                   
                   


                    string jsonData = JsonConvert.SerializeObject(data);
                    string ProductIdList = Protection.Encrypt(jsonData);
                    var arrayString2 = ChunksUpto(ProductIdList, 3500);

                    if (arrayString2.Count() == 1)
                    {


                        HttpContext.Response.Cookies.Append("StoreCart", ProductIdList, cookieOptions);

                    }
                    else
                    {
                        var count = 0;

                        foreach (var item in arrayString2)
                        {
                            if (count == 0)
                            {
                                HttpContext.Response.Cookies.Append("StoreCart", item, cookieOptions);
                                count++;
                            }
                            else
                            {
                                HttpContext.Response.Cookies.Append($"StoreCart{count}", item, cookieOptions);
                                count++;
                            }
                        }
                    }

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, responseText = "No Item in Cart list" });
                }
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }



        }



        [HttpGet]
        [Route("/Cart")]
        public IActionResult Cart()
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


        [HttpGet]
        [Route("/Checkout")]
        public IActionResult Checkout()
        {
            ViewBag.CollectionTime = BindCollections(GetGMTDateTime().ToString("dddd"));

			try
            {
                var checkBankHoliday = _bankHolidayRepository.GetAllbankHoliday().Any(x => x.SysTime == GetGMTDateTime().ToString("dd-MMM-yyyy"));

                var checkonlineOrder = _homeRepository.OrderOnlineOnOff().IsOrderOnline;
                if (!checkonlineOrder)
                {

                    ViewBag.ErrorMsg = "/images/kingTemprarayclosed.png";
                    return View("ResturantClosed");
                }
                
                if (GetGMTDateTime().ToString("dddd").ToLower() != "monday" || checkBankHoliday)
                {

                }
                else
                {
                    ViewBag.ErrorMsg = "/images/kingRestaurentClosed.png";
                    return View("ResturantClosed");
                }

				string cookieeData = "";

				var dataList = HttpContext.Request.Cookies.Keys.ToList();

				foreach (var item in dataList)
				{
					if (item.Contains("StoreCart"))
					{
						cookieeData += HttpContext.Request.Cookies[item];
					}
				} 

				string StoreCart = Protection.Decrypt(cookieeData);


                var data = JsonConvert.DeserializeObject<CartModel>(StoreCart);


                if (data == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (data.productIds.Count <= 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                    var placeOrder = new PlaceOrder();
                if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

                    var userData = _userRepository.GetUsersById(id);
                    var addresses = _userRepository.GetAllUsersAddress().ToList().Where(x=>x.UserId==userData.UserId).FirstOrDefault();

                    placeOrder.FullName = userData.UserName;
                    placeOrder.UserId = userData.UserId;
                    placeOrder.Email = userData.EmailAddress;
                    placeOrder.PhoneNo = userData.Phone;

					if (addresses != null)
                    {
                        if (string.IsNullOrEmpty(placeOrder.PhoneNo))
                        {
                            placeOrder.PhoneNo = addresses.ContactNo.ToString();
                        }
                        placeOrder.Country = addresses.Country;
                        placeOrder.County = addresses.State;
                        placeOrder.Town = addresses.Town;
                        placeOrder.Address = addresses.Addresss;
                        placeOrder.AddressType = addresses.AddreessType;
                        placeOrder.EIR = addresses.PostalCode.ToString();
                    }
                    return View(placeOrder);
                }
                else
                {
                    placeOrder.UserId = 0;
                    return View(placeOrder);
                }
            }
            catch (Exception)
            {

                return View();
            }
        }

        [HttpPost]
        [Route("/Checkout")]

        public async Task<IActionResult> CheckoutAsync(PlaceOrder placeOrder)
        {

            try
            {
                ViewBag.CollectionTime = BindCollections(GetGMTDateTime().ToString("dddd"));
				if (placeOrder.DeliveryType.ToLower() == "delivery")
                {
                    
                }
                else
                {
                    placeOrder.EIR = "-";
                    placeOrder.Address = "-";

				}
                var checkBankHoliday = _bankHolidayRepository.GetAllbankHoliday().Any(x => x.SysTime == GetGMTDateTime().ToString("dd-MMM-yyyy"));
                var checkonlineOrder = _homeRepository.OrderOnlineOnOff().IsOrderOnline;
                if (!checkonlineOrder)
                {

                    ViewBag.ErrorMsg = "/images/temporarily-closed.png";
                    return View("ResturantClosed");
                }
                if (GetGMTDateTime().ToString("dddd").ToLower() != "monday" || checkBankHoliday)
                {

                }
                else
                {
                    ViewBag.ErrorMsg = "/images/ResturantClosed.png";
                    return View("ResturantClosed");
                }
				string cookieeData = "";

				var dataList = HttpContext.Request.Cookies.Keys.ToList();

				foreach (var item in dataList)
				{
					if (item.Contains("StoreCart"))
					{
						cookieeData += HttpContext.Request.Cookies[item];
					}
				}

				string StoreCart = Protection.Decrypt(cookieeData);

				var data = JsonConvert.DeserializeObject<CartModel>(StoreCart);


                if (data == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (data.productIds.Count <= 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                var id = await UserCheck(placeOrder);
                if (id[0] <= 0)
                {
                    ViewBag.ErrorCheckOut = "Email Already Exists Kindly Login Your Account or Forget Your Password or Try with other Email";
                    return View(placeOrder);
                }
                
                placeOrder.UserId = id[0];

                if (placeOrder.DeliveryType.ToLower() == "collection")
                {
                    placeOrder.DeliveryId = -1;
               
                  


                }
                else
                {
                    if(placeOrder.DeliveryType.ToLower()=="delivery" && placeOrder.DeliveryId <= 0)
                    {
                        ViewBag.ErrorCheckOut = "Kindly Select Delivery Location";
                        return View(placeOrder);

                    }
                    else
                    {

                    }
                }
                if (placeOrder.PaymentType.ToLower() == "cash")
                {
                    TempData["isNewCustomer"] = id[1];

                    var json = JsonConvert.SerializeObject(placeOrder);
                    HttpContext.Session.SetString("PlaceOrder", json);

                    return RedirectToAction(nameof(ConfirmOrderCollection));
                }
                else
                {
                    var json = JsonConvert.SerializeObject(placeOrder);
                    HttpContext.Session.SetString("PlaceOrder", json);
                    TempData["isNewCustomer"] = id[1];
                    return RedirectToAction(nameof(ConfirmOrderCard));

                }




                



                return View(placeOrder);

            }
            catch (Exception exp)
            {
				ViewBag.CollectionTime = BindCollections(GetGMTDateTime().ToString("dddd"));
				ViewBag.ErrorCheckOut = exp.Message;
                return View(placeOrder);
            }
        }


        [HttpPost]
        public JsonResult BindTimeReservation(string date)
        {
            try
            {
                string dayName = Convert.ToDateTime(date).ToString("dddd");
           var listData=     BindCollections(dayName);

                return Json(new { success = true, responseText = listData });

            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }


        public IActionResult ConfirmOrderCollection()
        {

            try
            {
                var checkBankHoliday = _bankHolidayRepository.GetAllbankHoliday().Any(x => x.SysTime == GetGMTDateTime().ToString("dd-MMM-yyyy"));
                var checkonlineOrder = _homeRepository.OrderOnlineOnOff().IsOrderOnline;
                if (!checkonlineOrder)
                {

                    ViewBag.ErrorMsg = "/images/temporarily-closed.png";
                    return View("ResturantClosed");
                }
                if (GetGMTDateTime().ToString("dddd").ToLower() != "monday" || checkBankHoliday)
                {

                }
                else
                {
                    ViewBag.ErrorMsg = "/images/ResturantClosed.png";
                    return View("ResturantClosed");
                }
				string cookieeData = "";

				var dataList = HttpContext.Request.Cookies.Keys.ToList();

				foreach (var item in dataList)
				{
					if (item.Contains("StoreCart"))
					{
						cookieeData += HttpContext.Request.Cookies[item];
					}
				}

				string StoreCart = Protection.Decrypt(cookieeData);

				var data = JsonConvert.DeserializeObject<CartModel>(StoreCart);


                if (data == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (data.productIds.Count <= 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                if (TempData["isNewCustomer"] != null)
                {
                    ViewBag.IsNewUser = TempData["isNewCustomer"].ToString();
                }
                else
                {
                    ViewBag.IsNewUser = "0";

                }


                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

      
          public IActionResult ConfirmOrderCard()
        {

            try
            {
                var checkBankHoliday = _bankHolidayRepository.GetAllbankHoliday().Any(x => x.SysTime == GetGMTDateTime().ToString("dd-MMM-yyyy"));
                var checkonlineOrder = _homeRepository.OrderOnlineOnOff().IsOrderOnline;
                if (!checkonlineOrder)
                {

                    ViewBag.ErrorMsg = "/images/temporarily-closed.png";
                    return View("ResturantClosed");
                }
                if (GetGMTDateTime().ToString("dddd").ToLower() != "monday" || checkBankHoliday)
                {

                }
                else
                {
                    ViewBag.ErrorMsg = "/images/ResturantClosed.png";
                    return View("ResturantClosed");
                }

                if (TempData["isNewCustomer"] != null)
                {
                    ViewBag.IsNewUser = TempData["isNewCustomer"].ToString();
                }
                else
                {
                    ViewBag.IsNewUser = "0";

                }
				string cookieeData = "";

				var dataList = HttpContext.Request.Cookies.Keys.ToList();

				foreach (var item in dataList)
				{
					if (item.Contains("StoreCart"))
					{
						cookieeData += HttpContext.Request.Cookies[item];
					}
				}

				string StoreCart = Protection.Decrypt(cookieeData);


				var cartModel = JsonConvert.DeserializeObject<CartModel>(StoreCart);

            


                if (cartModel == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (cartModel.productIds.Count <= 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                var totalAmount = CalculateSum(cartModel);

                var deliveryCharges = _deliveryChargesRepository.GetAllDeliveryChargesById(cartModel.DeliveryId).Price;

                var GrandTotal = totalAmount + deliveryCharges;


                return View(GrandTotal);
            }
            catch (Exception)
            {

                throw;
            }

        }




        public async Task<List<int>> UserCheck(PlaceOrder placeOrder)
        {
            List<int> ids = new List<int>();
            if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
            {
                int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

                ids.Add(id);
                ids.Add(0);

                return ids;


            }
            else
            {
                Users user = _userRepository.UserGetByEmail(placeOrder.Email);

                if (user.UserId > 0)
                {
                    ids.Add(-1);
                    return ids;
                }
                else
                {
                    
                    ForgetPassword forgetPassword = new ForgetPassword();
                    Customers customers = new Customers();
                    customers.CustomerName = placeOrder.FullName;
                    customers.CustomerEmailAddress = placeOrder.Email;
                    customers.CustomerPassword = forgetPassword.GeneratePassword();
                    customers.CustomerPhone = placeOrder.PhoneNo;
                    int customerId = _customerDataAccess.AddCustomer(customers);

                    user = _userRepository.GetUsersById(customerId);
                    placeOrder.UserId = customerId;
					CookieOptions option = new CookieOptions();
					option.Secure = true;
					option.HttpOnly = true;
					option.Expires = DateTime.Now.AddDays(30);

					string userId = Protection.Encrypt(user.UserId.ToString());
					string userType = Protection.Encrypt(user.UserType.ToString());

					Response.Cookies.Append("KingOfCurriesUserId", userId, option);
					Response.Cookies.Append("KingOfCurriesUserType", userType, option);
					Response.Cookies.Append("KingOfCurriesUserName", user.UserName, option);

					HttpContext.Session.SetInt32("UserId", user.UserId);
					HttpContext.Session.SetInt32("UserType", user.UserType);
					HttpContext.Session.SetString("UserName", user.UserName);
					List<Claim> claims = new List<Claim>();
					if (user.UserType == 1)
					{
						claims = new List<Claim>()
				{
				  new  Claim(ClaimTypes.NameIdentifier,placeOrder.FullName),
				  new  Claim(ClaimTypes.Role,"Admin"),

				};
					}
					else if (user.UserType == 2)
					{
						claims = new List<Claim>() {
						new Claim(ClaimTypes.NameIdentifier, placeOrder.FullName),
				  new Claim(ClaimTypes.Role, "Client"),
				  };
					}
					else if (user.UserType == 3)
					{
						claims = new List<Claim>() {
						new Claim(ClaimTypes.NameIdentifier, placeOrder.FullName),
				  new Claim(ClaimTypes.Role, "Customer"),
				  };
					}
					ClaimsIdentity identity = new ClaimsIdentity(claims,
							   CookieAuthenticationDefaults.AuthenticationScheme);

					AuthenticationProperties properties = new AuthenticationProperties()
					{
						AllowRefresh = true,
						IsPersistent = true,

					};
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
							 new ClaimsPrincipal(identity), properties);

					EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(2);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", placeOrder.FullName);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#Email#", placeOrder.Email);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#UserPassword#", customers.CustomerPassword);

                    await _mail.SendMailAsync(placeOrder.Email, emailTemplate.MessageFor, emailTemplate.Subject);


                    ids.Add(customerId);
                    ids.Add(1);

                    return ids;



                }

            }

        }


       

        public async Task<IActionResult> PlaceOrderCollection()
        {
            try
            {
               
				var checkBankHoliday = _bankHolidayRepository.GetAllbankHoliday().Any(x => x.SysTime == GetGMTDateTime().ToString("dd-MMM-yyyy"));
				var checkonlineOrder = _homeRepository.OrderOnlineOnOff().IsOrderOnline;
				if (!checkonlineOrder)
				{

					ViewBag.ErrorMsg = "/images/temporarily-closed.png";
					return View("ResturantClosed");
				}
				if (GetGMTDateTime().ToString("dddd").ToLower() != "monday" || checkBankHoliday)
				{

				}
				else
				{
					ViewBag.ErrorMsg = "/images/ResturantClosed.png";
					return View("ResturantClosed");
				}


				if (string.IsNullOrEmpty(HttpContext.Session.GetString("PlaceOrder").ToString()))
				{
					throw new Exception("Internal server Error Occured");
				}

				if (string.IsNullOrEmpty(HttpContext.Request.Cookies["StoreCart"]))
				{
					throw new Exception("Internal server Error Occured");
				}

				if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
				{
					throw new Exception("Internal server Error Occured");
				}
				string cookieeData = "";

				var dataList = HttpContext.Request.Cookies.Keys.ToList();

				foreach (var item in dataList)
				{
					if (item.Contains("StoreCart"))
					{
						cookieeData += HttpContext.Request.Cookies[item];
					}
				}

				string StoreCart = Protection.Decrypt(cookieeData);


				var cartModel = JsonConvert.DeserializeObject<CartModel>(StoreCart);



			


				var data = HttpContext.Session.GetString("PlaceOrder");
				var placeOrder = new PlaceOrder();
				placeOrder = JsonConvert.DeserializeObject<PlaceOrder>(data);
				var check = await OrderConfirmedAsync(placeOrder,cartModel);
                    if (check.IsSuccess)
                    {
						return RedirectToAction(nameof(OrderComplete),new {id=check.text });
					}
                    else
                    {
                      return  RedirectToAction(nameof(Checkout));
                    }

                   


                


            }
            catch (Exception exp)
            {

                _homeRepository.LogXXXInsert(exp.Message);
                return RedirectToAction(nameof(Checkout));
            }
        }
           
        public async Task<IActionResult> StripePayment(string stripeToken, string stripeEmail, decimal PackageAmount)
        {
            ErrorModel errorModel = new ErrorModel();
            try
            {
                var checkBankHoliday = _bankHolidayRepository.GetAllbankHoliday().Any(x => x.SysTime == GetGMTDateTime().ToString("dd-MMM-yyyy"));
                var checkonlineOrder = _homeRepository.OrderOnlineOnOff().IsOrderOnline;
                if (!checkonlineOrder)
                {

                    ViewBag.ErrorMsg = "/images/temporarily-closed.png";
                    return View("ResturantClosed");
                }
                if (GetGMTDateTime().ToString("dddd").ToLower() != "monday" || checkBankHoliday)
                {

                }
                else
                {
                    ViewBag.ErrorMsg = "/images/ResturantClosed.png";
                    return View("ResturantClosed");
                }


                if (string.IsNullOrEmpty(HttpContext.Session.GetString("PlaceOrder").ToString()))
                {
                    throw new Exception("Internal server Error Occured");
                }

                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["StoreCart"]))
                {
                    throw new Exception("Internal server Error Occured");
                }

                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    throw new Exception("Internal server Error Occured");
                }

				string cookieeData = "";

				var dataList = HttpContext.Request.Cookies.Keys.ToList();

				foreach (var item in dataList)
				{
					if (item.Contains("StoreCart"))
					{
						cookieeData += HttpContext.Request.Cookies[item];
					}
				}

				string StoreCart = Protection.Decrypt(cookieeData);


				var cartModel = JsonConvert.DeserializeObject<CartModel>(StoreCart);



                var totalAmount = CalculateSum(cartModel);

                var deliveryCharges = _deliveryChargesRepository.GetAllDeliveryChargesById(cartModel.DeliveryId).Price;

                if (totalAmount >= 30)
                {
                    deliveryCharges = 0;
                }
                
                var GrandTotal = totalAmount + deliveryCharges;

                var StripePayment = GrandTotal * 100;



                var data = HttpContext.Session.GetString("PlaceOrder");
                var placeOrder = new PlaceOrder();
                placeOrder = JsonConvert.DeserializeObject<PlaceOrder>(data);

                string PaymentUniqueToken = GetGMTDateTime().ToString("yyyymmddhhmmss");
                int PaymentLogId = _homeRepository.PaymentLogAdd(PaymentUniqueToken, stripeToken, stripeEmail, "Stripe-Payment");

                PaymentUniqueToken = PaymentUniqueToken + PaymentLogId;
                errorModel.PaymentUniqueId = PaymentUniqueToken;

                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
                var root = builder.Build();
                var wowzaConfig = root.GetSection("Stripe");
                string pubKey = wowzaConfig["PublishKey"];
                string secKey = wowzaConfig["SecretKey"];


                Dictionary<string, string> Metadata = new Dictionary<string, string>()
                {
                    { "PaymentLogId", PaymentLogId.ToString() }
                };


                StripeConfiguration.ApiKey = secKey;
                var options = new ChargeCreateOptions
                {
                    Amount = (long)(StripePayment),
                    Currency = "EUR",
                    Source = stripeToken,
                    ReceiptEmail = stripeEmail,
                    Description = PaymentUniqueToken,
                    Metadata = Metadata,
                    
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);

                string StripeResponse = charge.StripeResponse.Content.ToString();
                switch (charge.Status)
                {
                    case "succeeded":
                        bool check = _homeRepository.PaymentLogUpdate(StripeResponse, "Success", PaymentLogId, 1, stripeEmail);
                        int PaymentId = _homeRepository.PaymentAdd(stripeToken, stripeEmail, PackageAmount, "Stripe", Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToString().ToInt32(), PaymentLogId, charge.PaymentMethod, charge.ReceiptUrl);
                        placeOrder.PaymentId= PaymentId;
                        placeOrder.PaymentType = "card";
                        var json = JsonConvert.SerializeObject(placeOrder);
                        HttpContext.Session.SetString("PlaceOrder", json);
                        var checkdata = await OrderConfirmedAsync(placeOrder, cartModel);
                        if (checkdata.IsSuccess)
                        {
							return RedirectToAction(nameof(OrderComplete), new { id = checkdata.text });
						}
                        else
                        {
                            return RedirectToAction(nameof(Checkout));
                        }
                        break;
                    case "failed":
                        bool check2 = _homeRepository.PaymentLogUpdate(StripeResponse, "Failed", PaymentLogId, 2, stripeEmail);
                        errorModel.ErrorCode = charge.FailureCode;
                        errorModel.ErrorMessage = charge.FailureMessage;
                        return RedirectToAction(nameof(PaymentFailed), errorModel);
                        break;
                }
                return RedirectToAction(nameof(ConfirmOrderCard));
            }
            catch (Exception exp)
            {
                errorModel.ErrorMessage = exp.Message;
				_homeRepository.LogXXXInsert(exp.Message);
				return RedirectToAction(nameof(PaymentFailed), errorModel);
            }
        }



        public async Task<Response> OrderConfirmedAsync( PlaceOrder placeOrder, CartModel cartModel)
        {

            var response = new Response();
           
         
            if (placeOrder.PaymentType.ToLower() == "cash")
            {
                placeOrder.PaymentId = -1;
            }
           

            var delivery = _deliveryChargesRepository.GetAllDeliveryChargesById(placeOrder.DeliveryId);

            placeOrder.DeliveryChargesId = delivery.DeliveryChargesId;
            placeOrder.DeliveryCharges = delivery.Price;
            if (placeOrder.DeliveryChargesId <= 0 && placeOrder.DeliveryType.ToLower() == "delivery")
            {
                response.IsSuccess = false;

				return response;
            }

		



            placeOrder.TotalAmount = CalculateSum(cartModel);

            if (placeOrder.TotalAmount >= 30)
            {
                placeOrder.DeliveryCharges = 0;
            }

            if (string.IsNullOrEmpty(placeOrder.Notes))
            {
                placeOrder.Notes = "-";
            }

            int invoiceId = _orderRepository.InsertInvoiceMain(placeOrder);

            foreach (var item in cartModel.productIds)
            {
                _orderRepository.InsertInvoiceDetail(invoiceId, item);
            }




            cartModel.productIds.Clear();

            cartModel = new CartModel();


            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = new DateTimeOffset(GetGMTDateTime().AddDays(-2));
            cookieOptions.HttpOnly = true;


            string jsonData = JsonConvert.SerializeObject(cartModel);
            string ProductIdList = Protection.Encrypt(jsonData);
			var dataList3 = HttpContext.Request.Cookies.Keys.ToList();

            int count = 0;
			foreach (var item in dataList3)
			{
                
				if (item.Contains("StoreCart"))
				{
                    if (count == 0)
                    {
						HttpContext.Response.Cookies.Append("StoreCart", ProductIdList, cookieOptions);
                        count++;
                    }
                    else
                    {
						HttpContext.Response.Cookies.Append($"StoreCart{count}", ProductIdList, cookieOptions);
						count++;
					}
		
				}
			}

            string emailTable = BindEmailOrderTable(invoiceId);
            
                EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(11);
            emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", placeOrder.FullName);
            emailTemplate.Subject = emailTemplate.Subject.Replace("#OrderDetails#", emailTable);
            await _mail.SendMailAsync(placeOrder.Email, emailTemplate.MessageFor, emailTemplate.Subject);



            response.IsSuccess = true;
            response.text = _orderRepository.GetAllOrdersMain(invoiceId).FirstOrDefault().OrderNo;
            return response;
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
                html += "<td style=\" font-weight:bold\">"+count+"</td>";
                html += "<td style=\" font-weight:bold\">"+item.ItemName+"<br/>"+item.SubName+"<br/>" + item.FreeName + "</td>";
                html += "<td style=\" font-weight:bold\">"+item.Quantity+"</td>";
                html += "<td style=\" font-weight:bold\">"+item.TotalPrice+"</td>";


                html += "</tr>";
                count++;
            }
            html += "</tbody>";
            html += "<tbody>";
            html += "<tr style=\" border:1px solid black\">";
            html += "<td colspan=\"3\" style=\" font-weight:bold;text-align:right\">Sub Total:</td>";
            html += "<td style=\" font-weight:bold\">" + Math.Round(orderMain.TotalAmount,2)+ "</td>";
            html += "</tr>";
            html += "<tr style=\" border:1px solid black\">";
            html += "<td colspan=\"3\" style=\" font-weight:bold;text-align:right\">Delivery Total:</td>";
            html += "<td style=\" font-weight:bold\">" + Math.Round(orderMain.DeliveryCharges,2) + "</td>";
            html += "</tr>";
            html += "<tr style=\" border:1px solid black\">";
            html += "<td colspan=\"3\" style=\" font-weight:bold;text-align:right\">Delivery Total:</td>";
            html += "<td style=\" font-weight:bold\">" + Math.Round(orderMain.GrandTotal,2) + "</td>";
            html += "</tr>";
            html += "</tbody>";
            html += "</table>";



            return html;
        }


        public async Task AuthUserAsync(Users user,bool KeepLoginIn)
        {

            CookieOptions option = new CookieOptions();
            option.Secure = true;
            option.HttpOnly = true;
            option.Expires = GetGMTDateTime().AddDays(30);

            string userId = Protection.Encrypt(user.UserId.ToString());
            string userType = Protection.Encrypt(user.UserType.ToString());

            Response.Cookies.Append("KingOfCurriesUserId", userId, option);
            Response.Cookies.Append("KingOfCurriesUserType", userType, option);
            Response.Cookies.Append("KingOfCurriesUserName", user.UserName, option);

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetInt32("UserType", user.UserType);
            HttpContext.Session.SetString("UserName", user.UserName);
            List<Claim> claims = new List<Claim>();
            if (user.UserType == 1)
            {
                claims = new List<Claim>()
                {
                  new  Claim(ClaimTypes.NameIdentifier,user.EmailAddress),
                  new  Claim(ClaimTypes.Role,"Admin"),

                };
            }
            else if (user.UserType == 2)
            {
                claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, user.EmailAddress),
                  new Claim(ClaimTypes.Role, "Client"),
                  };
            }
            else if (user.UserType == 3)
            {
                claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, user.EmailAddress),
                  new Claim(ClaimTypes.Role, "Customer"),
                  };
            }
            ClaimsIdentity identity = new ClaimsIdentity(claims,
                       CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = KeepLoginIn,

            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                     new ClaimsPrincipal(identity), properties);
        }



        public decimal CalculateSum(CartModel data)
        {
            List<ListCart> ListCart = new List<ListCart>();

            foreach (var ele in data.productIds)
            {
                ListCart listCart = new ListCart();
                listCart.Items = _itemRepository.GetAllItemsById(ele.ProductId);
                listCart.Items.Quantity = ele.Quantity;
                listCart.ProductId = ele.ProductId;
                listCart.Quantity = ele.Quantity;
                listCart.ProductSlug = ele.UniqueId;



                if (ele.SubProductId > 0)
                {
                    listCart.SubItemId = ele.SubProductId;
                    listCart.SubItems = _subItems.GetAllSubItems().Where(x => x.SubItemId == ele.SubProductId).FirstOrDefault();
                    listCart.SubItems.Quantity = listCart.Quantity;

                }
                else
                {
                    listCart.SubItemId = 0;

                }
                if (ele.FreeProductId > 0)
                {
                    listCart.FreeItemId = ele.FreeProductId;
                    listCart.FreeItems = _freeItems.GetAllFreeItems().Where(x => x.FreeItemId == ele.FreeProductId).FirstOrDefault();
                }
                else
                {
                    listCart.FreeItemId = 0;

                }

                ListCart.Add(listCart);
            }





            var totalPrice = Math.Round(ListCart.Where(x => x.SubItemId <= 0).Sum(x => x.Items.TotalPrice.ToDouble()), 2) + Math.Round(ListCart.Where(x => x.SubItemId > 0).Sum(x => x.SubItems.TotalPrice.ToDouble()), 2);

            return totalPrice.ToDecimal();
        }



        [Route("home/OrderComplete/{id}")]
  
        public IActionResult OrderComplete(string id)
        {
            try
            {
                if(string.IsNullOrEmpty(id))
                {
                    _homeRepository.LogXXXInsert("From First Methode");
					return RedirectToAction(nameof(Index));
				}
               
                var OrderMain=_orderRepository.GetAllOrdersMain(-1).Where(x=>x.OrderNo==id ).FirstOrDefault();

                if(OrderMain == null)
                {
					_homeRepository.LogXXXInsert("From 2nd Methode");
					return RedirectToAction(nameof(Index));
                }
                if (OrderMain.StatusId ==6 || OrderMain.StatusId==7)
                {
					_homeRepository.LogXXXInsert("From 3rd Methode");
					return RedirectToAction(nameof(Index));
				}


				if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
				{
					_homeRepository.LogXXXInsert("From 4rt Methode");
					return RedirectToAction(nameof(Index));
				}


				var UserId = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

                if(OrderMain.UserId != UserId)
                {
					_homeRepository.LogXXXInsert("From 5th Methode");
					return RedirectToAction(nameof(Index));
				}


				return View("OrderComplete",id);
            }
            catch (Exception exp)
            {
				_homeRepository.LogXXXInsert(exp.Message);
				return RedirectToAction(nameof(Index));
			}
        }

        public IActionResult PaymentFailed(ErrorModel errorModel)
        {
            return View(errorModel);
        }

        public JsonResult CustomerInvoice(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    return Json(new { code = -1, success = false, responseText = "error" });
                }


                var UserId =Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

                var Invoicemain = _orderRepository.GetAllOrdersMain(-1).ToList().Where(x => x.OrderNo == id).FirstOrDefault();

                if (Invoicemain == null)
                {
                    return Json(new { code = -1, success = false, responseText = "error" });
                }

                var InvoiceDetail = _orderRepository.GetAllOrderDetail(Invoicemain.OrderId).ToList();

                return Json(new { code = 2, success = true, invoiceMain = Invoicemain, invoiceDetail = InvoiceDetail });
            }
            catch (Exception exp)
            {
                return Json(new { code = 3, success = false, responseText = exp.Message });
            }

        }


		public JsonResult GetCustomerReview()
		{
			try
			{
                GenericCustomerReview genericCustomerReview = new GenericCustomerReview();
                var data = _homeRepository.GetAllCustomerReviews();

                if (data.Count > 0)
                {
                    genericCustomerReview.CustomerReviews= data.OrderBy(arg => Guid.NewGuid()).Take(10).OrderBy(x => x.Rating).ToList();
                }
                else
                {
                    genericCustomerReview.CustomerReviews = new List<CustomerReviews>();

				}

				string wwwRootPath = _hostingEnvironment.WebRootPath;
				string UploadedFolder = $"/images/Reviews/";
				var basePath = Path.Combine(wwwRootPath + UploadedFolder);
				var fileCount = (from file in Directory.EnumerateFiles(@$"{basePath}", "*.jpg", SearchOption.AllDirectories)
								 select file).Count();
			


				DirectoryInfo d = new DirectoryInfo(@$"{basePath}");
				FileInfo[] Files = d.GetFiles("*.png"); //Getting Text files


				foreach (FileInfo file in Files)
				{
					genericCustomerReview.Images.Add(					
						 UploadedFolder + file.Name
					);
				}

                genericCustomerReview.FilesLocation = basePath;



				return Json(new { success = true, responseText = genericCustomerReview });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}
		}



		private List<SelectListItem> BindCollections(string dayName)
        {
          

			var timesData = GetTimes(dayName);

            var toDay = GetGMTDateTime().ToString("dd-MMM-yyyy");
            var newDay = GetGMTDateTime().AddDays(1).ToString("dd-MMM-yyyy");


            DateTime start = Convert.ToDateTime($"{toDay} {timesData[0]}");
			DateTime end = Convert.ToDateTime($"{newDay} {timesData[1]}");

			int interval = 30;
			List<string> lstTimeIntervals = new List<string>();
			for (DateTime i = start; i < end; i = i.AddMinutes(interval))
              if (i > GetGMTDateTime())
                lstTimeIntervals.Add(dayName + " " + i.ToString("HH:mm"));
         		
			List<SelectListItem> selectListItems = new List<SelectListItem>();
			foreach (var item in lstTimeIntervals)
            {
				selectListItems.Add(new SelectListItem { Text = item, Value = item });
			}
            selectListItems.Add(new SelectListItem { Text = dayName + " " + timesData[1], Value = dayName + " " + timesData[1] });
            return selectListItems;

		}


        private List<string> GetTimes(string dayName)
        {
            List<string> times = new List<string>();

            string openingTime=string.Empty, closingTime = string.Empty;


			if (dayName != null)
            {
				switch (dayName) {
                case "Monday":
                    openingTime = "14:00"; //set 30 mints gap in opening hour
                    closingTime = "23:30"; //set 15 mints gap in closing hour
					break;
                case "Tuesday":
						openingTime = "14:00"; //set 30 mints gap in opening hour
						closingTime = "23:30"; //set 15 mints gap in closing hour
						break;
                case "Wednesday":
						openingTime = "14:00"; //set 30 mints gap in opening hour
						closingTime = "23:30"; //set 15 mints gap in closing hour
						break;
                case "Thursday":
						openingTime = "14:00"; //set 30 mints gap in opening hour
						closingTime = "23:30"; //set 15 mints gap in closing hour
						break;
                case "Friday":
						openingTime = "14:00"; //set 30 mints gap in opening hour
						closingTime = "23:30"; //set 15 mints gap in closing hour
						break;
                case "Saturday":
						openingTime = "14:00"; //set 30 mints gap in opening hour
						closingTime = "23:30"; //set 15 mints gap in closing hour
						break;
                case "Sunday":
						openingTime = "14:00"; //set 30 mints gap in opening hour
						closingTime = "23:30"; //set 15 mints gap in closing hour
						break;
				}

			}

			times.Add(openingTime);
			times.Add(closingTime);

			return times;

        }

        static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        private IEnumerable<string> SplitStringCart(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        private DateTime GetGMTDateTime()
        {
            int hour=_homeRepository.GetGMTTimeIncrement();
			
            DateTime MyTimeInWesternEurope2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "Romance Standard Time").AddHours(hour);
		

			return MyTimeInWesternEurope2;


		}


    }


}