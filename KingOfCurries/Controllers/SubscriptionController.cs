using Microsoft.AspNetCore.Mvc;

namespace KingofCurries.Controllers
{
	public class SubscriptionController : Controller
	{
		private readonly ISubscriptionsRepository _subscriptionsRepository;

		private readonly IEmailRepository _emailRepository;
		private readonly IMailService _mail;
        public SubscriptionController(ISubscriptionsRepository subscriptionsRepository, IEmailRepository emailRepository, IMailService mail)
        {
            _subscriptionsRepository = subscriptionsRepository;
            _emailRepository = emailRepository;
            _mail = mail;
        }

        public IActionResult Index()
		{
			return View();
		}


		[HttpPost]
		[Route("Subscription/Subscribed")]
		public async Task<JsonResult> InsertSubscriptionAsync(string emailAddress,string subType)
		{
			try
			{
				_subscriptionsRepository.InsertSubscriptions(emailAddress, subType);
				EmailTemplate emailTemplate = new EmailTemplate();
				if(subType== "News Letter")
                {
					emailTemplate=_emailRepository.GetEmailTemplateId(4);
                }
                else
                {
					emailTemplate = _emailRepository.GetEmailTemplateId(3);
				}
				emailTemplate.Subject = emailTemplate.Subject.Replace("#UserName#", "");
				

				await _mail.SendMailAsync(emailAddress, emailTemplate.MessageFor, emailTemplate.Subject);

				return Json(new { isSuccess = true, message = $"You Successfully Subscribed for {subType}" });

			}
			catch (Exception exp)
			{
				return Json(new { isSuccess = false, message = exp.Message });
			}

		}
	}
}
