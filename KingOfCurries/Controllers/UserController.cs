
using Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gazebolivesite.Controllers
{
    public class UserController : Controller
    {
    //    UserDataAccessLayer userData = new UserDataAccessLayer();
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }

    //    public JsonResult _AddUsers(string UserName,string UserEmail,string Pass, int UserType)
    //    {
    //        try
    //        {
    //            Users Users = new Users();
    //            Users.UserName = UserName;
    //            Users.EmailAddress = UserEmail;
    //            Users.Password = Pass;
    //            Users.CreatedBy = -1;
    //            Users.UserType = UserType;

    //            int res = userData.AddUsers(Users);
    //            return Json(new { success = true, res });
    //        }
    //        catch (Exception exp)
    //        {


    //            return Json(new { success = false, responseText = exp.Message });
    //        }

    //    }
    //    public IActionResult _ListUsers()
    //    {
    //        return View(new Users());
    //    }
    //    public JsonResult _ListUsersJson()
        
    //    {
    //        try
    //        {
    //            Users users = new Users();
    //            users.UserId = -1;
    //            users.UserName = "";
    //            users.EmailAddress = "";
    //            List<Users> listUsers = userData.GetUsers(users).ToList();
    //            return Json(listUsers);


    //        }
    //        catch (Exception exp)
    //        {
    //            return Json(new { success = false, responseText = exp.Message });
    //        }

    //    }
    //    public IActionResult _EditUsers(int id)
    //    {
    //        try
    //        {
    //            Users Users = new Users();
    //            Users = userData.GetUsersById(id);
    //            return PartialView("_EditUser", Users);

    //        }
    //        catch (Exception e)
    //        {
    //            ShowMessage("Edit Users Error" + e.Message);
    //            throw;
    //        }




    //    }
    //    public JsonResult _UpdateUsers(int usrId, string UserName,string UserEmail,string Pass, int UserType)
    //    {
    //        try
    //        {
    //            Users Users = new Users();
    //            Users.UserId = Convert.ToInt32(usrId);
    //            Users.UserName = UserName;
    //            Users.EmailAddress = UserEmail;
    //            Users.Password = Pass;
    //            Users.UpdatedBy = -2;
    //            Users.UserType = UserType;

    //            int res = userData.UpdateUsers(Users);
    //            return Json(new { success = true, res });
    //        }
    //        catch (Exception exp)
    //        {
    //            return Json(new { success = false, responseText = exp.Message });
    //        }
    //    }
    //    public JsonResult DeleteUsers(int id)
    //    {
    //        try
    //        {

    //            bool res = userData.DeleteUsers(id, 1);
    //            return Json(new { success = true, res });
    //        }
    //        catch (Exception exp)
    //        {

    //            return Json(new { success = false, responseText = exp.Message });
    //        }
    //    }


    //    private void ShowMessage(string exceptionMessage)
    //    {
    //        log.Info("Exception: " + exceptionMessage);
    //    }
    }
}
