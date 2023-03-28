using KingofCurries.Models;

using Microsoft.AspNetCore.Mvc;
using Services;

namespace KingofCurries.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BankHolidayController : Controller
	{
		private readonly IBankHolidayRepository _bankHolidayRepository;

		public BankHolidayController(IBankHolidayRepository bankHolidayRepository)
		{
			_bankHolidayRepository = bankHolidayRepository;
		}

		public IActionResult Index()
		{
			return View();
		}




        [HttpPost]
        public JsonResult AddBankHoliday(BankHoliday bankHoliday)
        {
            try
            {

                _bankHolidayRepository.Insert(bankHoliday);

                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        public JsonResult ListBankHoliday()
        {
            try 
            {


                List<BankHoliday> bankHoliday = _bankHolidayRepository.GetAllbankHoliday().ToList();

                return Json(new { code = true, jsonText = bankHoliday });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        public JsonResult DeleteBankHoliday(int id)
        {
            try
            {



                bool res = _bankHolidayRepository.DeletebankHoliday(id, -1);
                return Json(new { success = true, responseText = "Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

        public IActionResult EditBankHolidayById(int id)
        {
            try
            {



                BankHoliday res = _bankHolidayRepository.GetAllbankHolidayById(id);
                return PartialView("_EditBankHoliday", res); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

        [HttpPost]
        public JsonResult UpdateBankHoliday(BankHoliday bankHoliday)
        {
            try
            {


                bankHoliday.UserId = -1;



                bool check = _bankHolidayRepository.UpdatebankHoliday(bankHoliday);


                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }
    }
}
