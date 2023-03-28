using KingofCurries.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Security.Claims;

namespace KingofCurries.Controllers
{

    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
       
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

		[Authorize(Roles = "Admin")]
		public IActionResult Index()
        {

            return View();
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public JsonResult AddUsers(Users users)
        {
            try
            {

                _userRepository.InsertUsers(users);

                return Json(new { code = true, jsonText = "User Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }



		[Authorize(Roles = "Admin")]
		public JsonResult ListUsers()
        {
            try
            {


                List<Users> users = _userRepository.GetUsers().ToList();

                return Json(new { code = true, jsonText = users });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


		[Authorize(Roles = "Admin")]
		public IActionResult EditUserById(int id)
        {
            try
            {



                Users users = _userRepository.GetUsersById(id);
                return PartialView("_EditUser", users); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public JsonResult UpdateUsers(Users users)
        {
            try
            {


                users.UpdatedBy = -1;



                bool check = _userRepository.UpdateUsers(users);


                return Json(new { code = true, jsonText = "User Updated Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


		[Authorize(Roles = "Admin")]
		public JsonResult DeleteUser(int id)
        {
            try
            {



                bool res = _userRepository.DeleteUsers(id, -1);
                return Json(new { success = true, responseText = "User Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

        [HttpGet]
        [Route("/Users/Login/{id?}")]
        public IActionResult Login(string? id )
        {
            if (string.IsNullOrEmpty(HttpContext.Request.Cookies["KingOfCurriesUserId"]))
            {
                return View();
            }
            else
            {
                if (Protection.Decrypt(HttpContext.Request.Cookies["KingOfCurriesUserType"]) != "2")
                {
                    return View();
                }
            }


                return RedirectToAction("TodayOrders", "Clients");
                
        }

        [HttpPost]
   
        public IActionResult Login(ClientLogin clientLogin)
        {
            try
            {
                Users user = _userRepository.UserLogin(clientLogin.UserName, clientLogin.Password);


                if (user.UserId > 0)
                {
                    CookieOptions option = new CookieOptions();
                    option.Secure = true;
                    option.HttpOnly = true;
                    option.Expires = DateTime.Now.AddDays(60);

                    string userId = Protection.Encrypt(user.UserId.ToString());
                    string userType = Protection.Encrypt(user.UserType.ToString());

                    Response.Cookies.Append("KingOfCurriesUserId", userId, option);
                    Response.Cookies.Append("KingOfCurriesUserType", userType, option);
                    Response.Cookies.Append("KingOfCurriesUserName", user.UserName, option);

                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetInt32("UserType", user.UserType);
                    HttpContext.Session.SetString("UserName", user.UserName);

                    return RedirectToAction("Index", "Clients");
                }

                else
                {
                    ViewBag.ErrorMsg = "Invalid User Name or Password";

                    return View(clientLogin);
                }


            }
            catch (Exception exp)
            {

                ViewBag.ErrorMsg= exp.Message;
                return View(clientLogin);
            }
     
        }

    }
}
