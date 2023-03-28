using _Helper;

using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KingofCurries.Models;
using Newtonsoft.Json;
using System.Web;

namespace KingofCurries.Controllers
{

    
    public class CustomerController : Controller
    {
        private readonly ICustomerDataAccessLayer customerDataAccess;
        private readonly IEmailRepository _emailRepository;
        private readonly IMailService _mail;

        public CustomerController(ICustomerDataAccessLayer _customerDataAccess, IEmailRepository emailRepository, IMailService mail)
        {
            customerDataAccess = _customerDataAccess;
            _emailRepository = emailRepository;
            _mail = mail;
        }

        //EmailTemplateDataAccessLayer emailTemplateDataAccessLayer = new EmailTemplateDataAccessLayer();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        
        
        public async Task<JsonResult> RegistrationAsync(Customers customers)
        {
            try
            {
                if (ModelState.IsValid)
                {
					bool containsHTML = (customers.CustomerName != HttpUtility.HtmlEncode(customers.CustomerName));

                    if(containsHTML)
                    {
                        throw new Exception("Name provided is invalid");
                    }

					int id = customerDataAccess.AddCustomer(customers);
                    if (id > 0)
                    {

                        CookieOptions option = new CookieOptions();
                        option.Secure = true;
                        option.HttpOnly = true;
                        option.Expires = DateTime.Now.AddDays(30);

                        string userId = Protection.Encrypt(id.ToString());
                        string userType = Protection.Encrypt("3");

                        Response.Cookies.Append("KingOfCurriesUserId", userId, option);
                        Response.Cookies.Append("KingOfCurriesUserType", userType, option);
                        Response.Cookies.Append("KingOfCurriesUserName", customers.CustomerName, option);


                    }

                    //EmailHelper emailHelper = new EmailHelper();
                    //EmailTemplate emailTemplate = emailTemplateDataAccessLayer.GetEmailTemplateId(2);

                    //string Message = emailTemplate.Subject;

                    //Message = Message.Replace("#EmailAddress#", customers.CustomerEmailAddress);
                    //Message = Message.Replace("#UserName#", customers.CustomerName);
                    //Message = Message.Replace("#Password#", customers.CustomerPassword);
                    //Status sts = emailHelper.SendEmail(emailTemplate.FromEmail, customers.CustomerEmailAddress, customers.CustomerName, "Thanks for Signup", Message);
                    //if (sts.Success)
                    //{
                    //}

                    EmailTemplate emailTemplate = _emailRepository.GetEmailTemplateId(2);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#Email#", customers.CustomerEmailAddress);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#UserPassword#", customers.CustomerPassword);

                    emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", customers.CustomerName);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#Email#", customers.CustomerEmailAddress);
                    emailTemplate.Subject = emailTemplate.Subject.Replace("#UserPassword#", customers.CustomerPassword);



                    await _mail.SendMailAsync(customers.CustomerEmailAddress, emailTemplate.MessageFor, emailTemplate.Subject);
                    return Json(new { success = true, redirect = Url.Action("Index", "Website") });
                }
                else
                {
                    string errors = JsonConvert.SerializeObject(ModelState.Values
    .SelectMany(state => state.Errors)
    .Select(error => error.ErrorMessage));
                    return Json(new { success = false, responseText = errors });
                }

                
            }
            catch (Exception exp)
            {

                return Json(new { success = false,responseText=exp.Message });

            }
        }

        public void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMinutes(60);

            Response.Cookies.Append(key, value, option);
        }
    }
}
