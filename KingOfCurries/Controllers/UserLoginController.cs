using _Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Http;
using OtpNet;

using ExtensionMethods;
using Models;
using KingofCurries.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Metadata;
using Services;
using Repository;

namespace Gazebolivesite.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly ICustomerDataAccessLayer customerDataAccessLayer;
        private readonly IUserRepository userDataAccessLayer;
        private readonly IEmailRepository _emailRepository;
        private readonly IMailService _mail;
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryChargesRepository _deliveryChargesRepository;
        private readonly IHomeRepository _homeRepository;

		public UserLoginController(ICustomerDataAccessLayer _customerDataAccessLayer,IHomeRepository homeRepository,IOrderRepository orderRepository, IUserRepository _userDataAccessLayer, IEmailRepository emailRepository, IMailService mail,IDeliveryChargesRepository deliveryChargesRepository)
        {
            customerDataAccessLayer = _customerDataAccessLayer;
            userDataAccessLayer = _userDataAccessLayer;
            _emailRepository = emailRepository;
            _mail = mail;
			_deliveryChargesRepository = deliveryChargesRepository;
            _orderRepository = orderRepository;
            _homeRepository = homeRepository;
		}


        //  EmailTemplateDataAccessLayer emailTemplateDataAccessLayer = new EmailTemplateDataAccessLayer();
        //  AssignStoretoUsersDataAccessLayer assignStoretoUsers = new AssignStoretoUsersDataAccessLayer();
        [HttpPost]
        [Route("login")]
        public async Task<JsonResult> loginAsync(string UserName, string Password, Boolean KeepLoginIn)
        {
            try
            {



                Users user = userDataAccessLayer.UserLogin(UserName, Password);


                if (user.UserId > 0)
                {
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
                  new  Claim(ClaimTypes.NameIdentifier,UserName),
                  new  Claim(ClaimTypes.Role,"Admin"),

                };
                    }
                    else if (user.UserType == 2)
                    {
                        claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, UserName),
                  new Claim(ClaimTypes.Role, "Client"),
                  };
                    }
                    else if (user.UserType == 3)
                    {
                        claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, UserName),
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

                    return Json(new { success = true, redirect = Url.Action("Index", "Website") });
                }

                else
                {
                    return Json(new { success = false, responseText = "Invalid UserName or Password" });
                }




            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });

            }

        }


      

       


        [Route("MyAccount")]
        [Authorize]
        public IActionResult MyAccount()
        {
            try
            {

               
				if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
				{

					int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32(); 
					if (id > 0)
					{

                        var userData=userDataAccessLayer.GetUsersById(id);

                        GenericUser genericUser = new GenericUser();
						genericUser.Users= userData;
                        return View(genericUser);
                


                        //if (user.UserId > 0)
                        //{
                        //    PaymentDataAccessLayer paymentDataAccessLayer = new PaymentDataAccessLayer();
                        //    AssignStoretoUser assignStoretoUser = assignStoretoUsers.GetSingleUserAssignStore(user.UserId).FirstOrDefault();
                        //    user.Invoice = paymentDataAccessLayer.InvoiceTableDataGetDataByInvoiceId(-1, user.UserId).ToList();
                        //    if (assignStoretoUser != null)
                        //    {

                        //        user.StoreId = assignStoretoUser.StoreId;
                        //    }
                        //    else
                        //    {
                        //        user.StoreId = 0;
                        //    }

                        //    return View(user);
                        //}



                    }
				}

				return RedirectToAction("Index", "Website");
			}
            catch (Exception exp)
            {

                TempData["MasterError"] = "Error! " + exp.Message;
                return RedirectToAction("Index", "Website");


            }

        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            try
            {
                CookieOptions option = new CookieOptions();
                option.Secure = true;
                option.HttpOnly = true;
                option.Expires = DateTime.Now.AddDays(-2);
                Response.Cookies.Append("KingOfCurriesUserId", "", option);
                Response.Cookies.Append("KingOfCurriesUserType", "", option);
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");

            }


        }

        [HttpGet]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword()
        {
            try
            {
                return View();
            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg = "Error Occurreds! " + exp.Message;
                return View();
            }

        }

        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync(string Email)
        {
            Users user = new Users();
            string UserName = "";
            try
            {


                user = userDataAccessLayer.UserGetByEmail(Email);
                if (user.UserId > 0)
                {
                  //  EmailHelper emailHelper = new EmailHelper();
                    ForgetPassword forgetPassword = new ForgetPassword();
                    string secretkey = forgetPassword.GeneratePasswordForget();
                    var bytes = Base32Encoding.ToBytes(secretkey);
                    var otp = new Totp(bytes);
                    var result = otp.ComputeTotp(DateTime.UtcNow);


                    EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(6);

                    emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", user.UserName);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#Code#", result);

                 bool sts=   await _mail.SendMailAsync(Email, emailTemplate.MessageFor, emailTemplate.Subject);
                    if (sts)
                    {
                        TempData["Email"] = Email;
                            TempData["OTPCode"] = result;
                            ViewBag.EmailShow = 2;
                           ViewBag.EmailAddress = Email;
                        TempData["UserId"] = user.UserId;
                        TempData["UserName"] = UserName;
                        return View("ConfirmOtpCode");
                    }
                    else
                    {
                        ViewBag.Error = 3;
                        TempData["ErrorMessage"] = "Error Occured";
                        ViewBag.EmailAddress = Email;
                        TempData["UserId"] = user.UserId;
                        ViewBag.ErrorMsg3 = "Error Occured";
                        TempData["UserName"] = UserName;
                  
                        return View();

                    }
                   
                }
                else
                {
                    TempData["UserId"] = user.UserId;
                   
                    TempData["UserName"] = UserName;
                    ViewBag.Error = 2;
                    ViewBag.ErrorMsg3 = "Error Occured";
                    return View();
                }
              

            }
            catch (Exception exp)
            {
                TempData["UserName"] = UserName;
                ViewBag.ErrorMsg = "Error occured " + exp.Message;
                return View();


            }

        }


        [HttpPost]
        [Route("ConfirmOtpCode")]
        public IActionResult ConfirmOtpCode(string Input1, string Input2, string Input3, string Input4, string Input5, string Input6)
        {
            try
            {

                string Code = Input1 + Input2 + Input3 + Input4 + Input5 + Input6;
                int OtpCode = Convert.ToInt32(Code);
                string Email = TempData["Email"].ToString();
                int ConfirmOtpCode = Convert.ToInt32(TempData["OTPCode"]);
                if (OtpCode == ConfirmOtpCode)
                {
                    ViewBag.Email = Email;

                    TempData["UserName"] = TempData["UserName"];
                    TempData["UserId"] = TempData["UserId"];
                    TempData["Email"] = Email;
                    return View("ResetPassword");
                }
                else
                {
                    ViewBag.EmailShow = 1;
                    TempData["Email"] = TempData["Email"];
                    ViewBag.EmailAddress = "aaaaaa";
                    ViewBag.Error = 2;
                    TempData["UserId"] = TempData["UserId"];
                    TempData["UserName"] = TempData["UserName"];
                    return View();
                }


            }
            catch (Exception exp)
            {
                TempData["Email"] = TempData["Email"];
                TempData["UserId"] = TempData["UserId"];
                ViewBag.ErrorMsg = "Error occured " + exp.Message;
                return View();
            }
        }
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string hdnEmail, string newPassword)
        {
            try
            {
                int UserId = Convert.ToInt32(TempData["UserId"]);
                string Email = TempData["Email"].ToString();
                userDataAccessLayer.ResetUserPassword(UserId, newPassword);
                return View("PasswordResetMessage");
            }
            catch (Exception exp)
            {
                TempData["UserName"] = TempData["UserName"].ToString();
                TempData["UserId"] = TempData["UserId"];
                TempData["Email"] = TempData["Email"];
                ViewBag.ErrorMsg = "Error occured " + exp.Message;
                return View();
            }  }
		[HttpPost]
		public JsonResult AddAddress1(Address address)
		{
			try
			{
                if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
					int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();
                    address.UserId= id;
					userDataAccessLayer.UserInsert(address);
                }
                else
                {
                    return Json(new { code = false, jsonText = "Kindly logout and login again" });

				}


				return Json(new { code = true, address });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}

        [HttpPost]
        public JsonResult UpdateAddress(Address address)
        {
            try
            {

                int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();
                address.UserId = id;
                userDataAccessLayer.UpdateAddress(address);


                return Json(new { code = true, jsonText = "Updated Address Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        [HttpGet]
		public JsonResult ListAddres()

		{
			try
			{
                if (!string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
					int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

					List<Address> addresses = userDataAccessLayer.GetAllUsersAddress().ToList().FindAll(x=> x.UserId==id);

                    return Json(new { code = true, jsonText = addresses });
                }
                else
                {
                    return Json(new { code = false, jsonText = new List<Address>() });
                }


			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}


		public JsonResult DeleteAddres(int id)
		{
			try
			{



				bool res = userDataAccessLayer.Delete(id, -1);
				return Json(new { success = true, responseText = "Deleted Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}

      
        public IActionResult EditUserAddress(int id)
        {
            try
            {

                var address=userDataAccessLayer.GetAllUsersAddress().FirstOrDefault(x=>x.Id==id);

                return PartialView("_EditAddress", address);

            }
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}
		}

		public JsonResult ListDeliveryCharges()
		{
			try
			{


				List<DeliveryCharges> deliveryCharges = _deliveryChargesRepository.GetAllDeliveryCharges().ToList();

				return Json(new { code = true, jsonText = deliveryCharges });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}

		public JsonResult AddDeliveryCharges(int id)
		{
			try
			{
				CookieOptions cookieOptions = new CookieOptions();
				cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMonths(6));
				cookieOptions.HttpOnly = true;

				var deliveryCharges = _deliveryChargesRepository.GetAllDeliveryCharges().FirstOrDefault(x=>x.DeliveryChargesId==id);

				string StoreCart = Protection.Decrypt(HttpContext.Request.Cookies["StoreCart"]);

				var data = JsonConvert.DeserializeObject<CartModel>(StoreCart);

                data.DeliveryCharges = deliveryCharges.Price;
                data.DeliveryId = id;
				string jsonData = JsonConvert.SerializeObject(data);
				string ProductIdList = Protection.Encrypt(jsonData);
				HttpContext.Response.Cookies.Append("StoreCart", ProductIdList, cookieOptions);

				string StoreCart2 = Protection.Decrypt(HttpContext.Request.Cookies["StoreCart"]);

				var data1 = JsonConvert.DeserializeObject<CartModel>(StoreCart);

				return Json(new { code = true, jsonText = deliveryCharges,deliverycharges=data1.DeliveryCharges,subTotal=data1.TotalPrice,grandTotal=data1.GrandTotal });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}

        [HttpPost]
        public JsonResult ChangeDeliveryType(int id,string dataType)
		{
			try
			{
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = new DateTimeOffset(DateTime.Now.AddMonths(6));
                cookieOptions.HttpOnly = true;
                var deliveryCharges = _deliveryChargesRepository.GetAllDeliveryCharges().FirstOrDefault(x => x.DeliveryChargesId == id);
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



				decimal deliveryprice = 0;
                int deliveryId = -1;
                if (dataType.ToLower() == "delivery")
                {
                    if (id <= 0)
                    {
                        
                    }
                    else
                    {
                        deliveryprice = deliveryCharges.Price;
                        deliveryId = id;
                    }

                }
                else
                {
                    deliveryprice = 0;
                    deliveryId = -1;
                }

                var data = JsonConvert.DeserializeObject<CartModel>(StoreCart);
                data.DeliveryCharges = deliveryprice;
                data.DeliveryId = deliveryId;

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


				

				return Json(new { code = true, jsonText = deliveryCharges,deliverycharges= deliveryprice });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}
		}

        public JsonResult TodayOrdersGet()
        {
            try
            {


                //List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == DateTime.Now.ToString("dd-MMM-yyyy")).ToList();
                //orders = orders.Where(x => x.StatusId != 6 || x.StatusId != 7).ToList();

                int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

				List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).ToList().FindAll(x => x.UserId == id && x.OrderDate.ToString("dd-MMM-yyyy") == DateTime.Now.ToString("dd-MMM-yyyy") );
				orders = orders.Where(x => x.StatusId != 6 ).ToList();
				orders = orders.Where(x => x.StatusId != 7 ).ToList();

               
					return Json(new { code = true, jsonText = orders });
                
                
              
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }




        }
        public JsonResult GetAllOrders()
        {
            try
            {
                int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

                List<OrdersMain> deliveryCharges = _orderRepository.GetAllOrdersMain(-1).ToList().FindAll(x => x.UserId == id);

                return Json(new { code = true, jsonText = deliveryCharges });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }

        public JsonResult ListOfReservations()
        {
            try
            {

                int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();
                List<Reservation> reservations = _homeRepository.GetAllReservation().Where(x => x.UserId == id).ToList();

                return Json(new { code = true, jsonText = reservations });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }

		static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
		{
			for (int i = 0; i < str.Length; i += maxChunkSize)
				yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
		}

		public JsonResult TodayOrdersGetToast()
		{
			try
			{


				//List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).Where(x => x.OrderDate.ToString("dd-MMM-yyyy") == DateTime.Now.ToString("dd-MMM-yyyy")).ToList();
				//orders = orders.Where(x => x.StatusId != 6 || x.StatusId != 7).ToList();

				int id = Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]).ToInt32();

				List<OrdersMain> orders = _orderRepository.GetAllOrdersMain(-1).ToList().FindAll(x => x.UserId == id && x.OrderDate.ToString("dd-MMM-yyyy") == DateTime.Now.ToString("dd-MMM-yyyy"));
				orders = orders.Where(x => x.StatusId != 6).ToList();
				orders = orders.Where(x => x.StatusId != 7).ToList();



				if (orders.Count > 0)
                {
                    var OrderData=orders.OrderBy(x=>x.RemainingTime).ToList().FirstOrDefault();
					return Json(new { code = true, jsonText = OrderData });
                }
                else
                {
					return Json(new { code = false, jsonText = "" });
				}

				



			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}




		}

        [HttpPost]
        public JsonResult ChangeUserPassword( string currentPassword, string Password, string ConfirmPassword)
        {
            try
            {
                int UserId = 0;
                if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
                {
                    return Json(new { success = false, responseText ="Kindly Logout and login again" });
                }
                UserId=Convert.ToInt32(Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserId"]));

                if (!(Password == ConfirmPassword))
                {
                    return Json(new { success = false, responseText = "Sorry Your Password Does not Match" });
                }
                else if (!(Password.Length > 8 || Password.Any(c => char.IsDigit(c)) || Password.Any(c => char.IsUpper(c)) || Password.Any(c => char.IsLower(c) || Password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1)))
                {
                    return Json(new { success = false, responseText = "Sorry Your Password is InValid" });
                }

                var data = userDataAccessLayer.UpdateUserPassowrd(UserId,  currentPassword, Password);
                if (data)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, responseText = "Your provide invalid current password" });
                }

            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }



    }
}
