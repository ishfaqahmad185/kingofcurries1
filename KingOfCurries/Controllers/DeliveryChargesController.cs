using KingofCurries.Models;

using Microsoft.AspNetCore.Mvc;
using Services;
using System.Collections.Generic;

namespace KingofCurries.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeliveryChargesController : Controller
    {
        private readonly IDeliveryChargesRepository _deliveryChargesRepository;
         
        public DeliveryChargesController(IDeliveryChargesRepository deliveryChargesRepository)
        {
            _deliveryChargesRepository = deliveryChargesRepository;
        }

        public IActionResult Index()
        {
            return View();
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



        [HttpPost]
        public JsonResult AddDeliveryCharges(DeliveryCharges deliveryCharges)
        {
            try
            {
                deliveryCharges.UserId = -1;

                _deliveryChargesRepository.Insert(deliveryCharges);

                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }

        public JsonResult DeleteDeliveryCharges(int id)
        {
            try
            {



                bool res = _deliveryChargesRepository.DeleteDeliveryCharges(id, -1);
                return Json(new { success = true, responseText = "Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

        public IActionResult EditDeliveryChargesById(int id)
        {
            try
            {

               

                DeliveryCharges res = _deliveryChargesRepository.GetAllDeliveryChargesById(id);
                return PartialView("_EditDeliveryCharges", res); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }



        [HttpPost]
        public JsonResult UpdateDeliveryCharges(DeliveryCharges deliveryCharges)
        {
            try
            {


                deliveryCharges.UserId = -1;



                bool check = _deliveryChargesRepository.UpdateDeliveryCharges(deliveryCharges);


                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }

    }
}
