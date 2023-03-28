using KingofCurries.Models;
using HarrysRestro.Services;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Services;
using System.Drawing.Text;

namespace KingofCurries.Controllers
{
    public class CompanyProfileController : Controller
    {
        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public CompanyProfileController(ICompanyProfileRepository companyProfileRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        { 
            _companyProfileRepository= companyProfileRepository;
            _hostingEnvironment = hostingEnvironment;


        }
        public IActionResult Index()
        {
            return View();
           
        }

		public IActionResult AddCompanyProfile(CompanyProfile companyProfile)
		{
			try
			{



				var fileName = Path.GetFileNameWithoutExtension(companyProfile.CompanyImage.FileName);
				var fileExtension = Path.GetExtension(companyProfile.CompanyImage.FileName);
				var Image = $"{fileName}_{Guid.NewGuid().ToString()}.{fileExtension}";

				string wwwRootPath = _hostingEnvironment.WebRootPath;
				string UploadedFolder = $"/Uploadimages/CompanyProfileImages/";




				var basePath = Path.Combine(wwwRootPath + UploadedFolder);



				bool basePathExists = System.IO.Directory.Exists(basePath);



				if (!basePathExists) Directory.CreateDirectory(basePath);



				var filePath = Path.Combine(basePath, Image);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					companyProfile.CompanyImage.CopyTo(stream);


				}

				string imageURL = UploadedFolder + Image;


				companyProfile.UserId = -1;
				companyProfile.CompanyLogo = imageURL;
				

				bool check = _companyProfileRepository.AddCompanyProfile(companyProfile);


				return Json(new { code = true, jsonText = "Added Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { code = false, jsonText = exp.Message.ToString() });
			}

		}
		public JsonResult ListCompanyProfile()
        {
            try
            {


                List<CompanyProfile> companyProfiles = _companyProfileRepository .GetAllCompanyProfile().ToList();

                return Json(new { code = true, jsonText = companyProfiles });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }
      
      

        public IActionResult EditCompanyProfileById(int id)
        {
            try
            { 



                CompanyProfile edit = _companyProfileRepository.GetCompanyProfileById(id);
                return PartialView("_EditCompanyProfile", edit); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }
        [HttpPost]
        public JsonResult UpdateCompanyProfile(CompanyProfile companyProfile)
        {
            try
            {
                if (companyProfile.CompanyImage != null)
                {




                    var fileName = Path.GetFileNameWithoutExtension(companyProfile.CompanyImage.FileName);
                    var fileExtension = Path.GetExtension(companyProfile.CompanyImage.FileName);
                    var Image = $"{fileName}_{Guid.NewGuid().ToString()}.{fileExtension}";

                    string wwwRootPath = _hostingEnvironment.WebRootPath;
                    string UploadedFolder = $"/Uploadimages/CompanyProfileImages/";




                    var basePath = Path.Combine(wwwRootPath + UploadedFolder);



                    bool basePathExists = System.IO.Directory.Exists(basePath);



                    if (!basePathExists) Directory.CreateDirectory(basePath);



                    var filePath = Path.Combine(basePath, Image);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        companyProfile.CompanyImage.CopyTo(stream);


                    }

                    string imageURL = UploadedFolder + Image;
                    companyProfile.CompanyLogo = imageURL;

                }


                companyProfile.UserId = -1;



                bool check = _companyProfileRepository.UpdateCompanyProfile(companyProfile);


                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


		public JsonResult DeleteCompanyProfile(int id)
		{
			try
			{

				bool res = _companyProfileRepository.DeleteCompany(id, -1);
				return Json(new { success = true, responseText = "Deleted Successfully" });
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}


		}
	}


}

