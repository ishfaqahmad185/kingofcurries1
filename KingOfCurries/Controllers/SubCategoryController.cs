using KingofCurries.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace KingofCurries.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubCategoryController : Controller
    {
       private readonly ISubCategoryRepository _subCategoryDAL;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SubCategoryController(ISubCategoryRepository subCategoryDAL,ICategoryRepository categoryRepository ,IHostingEnvironment hostingEnvironment)
        {
            _subCategoryDAL = subCategoryDAL;
            _hostingEnvironment = hostingEnvironment;
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            LoadCategoryDDL();

            return View();
       
        }


        [HttpPost]
        
        public IActionResult AddSubCategory(SubCategory subCategory)
        {
          



            try
            {


   
                var fileName = Path.GetFileNameWithoutExtension(subCategory.image.FileName);
                var fileExtension = Path.GetExtension(subCategory.image.FileName);
                var Image = $"{fileName}_{Guid.NewGuid().ToString()}.{fileExtension}";

                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string CatName = _categoryRepository.GetCategoryById(subCategory.CategoryId).CategoryName;
                string UploadedFolder = $"/Uploadimages/subcategory/{CatName}/";
                


                var basePath = Path.Combine(wwwRootPath + UploadedFolder);
              


                bool basePathExists = System.IO.Directory.Exists(basePath);
             

                if (!basePathExists) Directory.CreateDirectory(basePath);
               


                var filePath = Path.Combine(basePath, Image);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    subCategory.image.CopyTo(stream);


                }
              


              


                subCategory.Slug=$"{subCategory.SubCategoryName.Replace(" ","-")}-{Guid.NewGuid()}";
                 
                subCategory.ImageUrl = UploadedFolder + Image;
                subCategory.CreatedBy = 1;

                bool check = _subCategoryDAL.InsertSubCategory(subCategory);


                subCategory.Slug=$"{subCategory.SubCategoryName.Replace(" ", "-")}-{Guid.NewGuid()}";
        




                return Json(new { code = true, jsonText = "SubCategory Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        public JsonResult ListSubCAtegory()
        {
            try
            {
                List<SubCategory> freeItems = _subCategoryDAL.GetAllSubCategory().ToList();

                return Json(new { code = true, jsonText = freeItems });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }



        public void LoadCategoryDDL()
        {


            IEnumerable<Category> countyList = _categoryRepository.GetAllCategory();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in countyList)
            {
                selectListItems.Add(new SelectListItem { Text = item.CategoryName.ToString(), Value = item.CategoryId.ToString() });
            }
            ViewBag.Category = selectListItems;

        }

		

		public IActionResult EditSubCategoryById(int id)
		{
			try
			{

                LoadCategoryDDL();

				SubCategory SubCat = _subCategoryDAL.GetSubCategoryById(id);
				return PartialView("_EditSubCategory", SubCat); ;
			}
			catch (Exception exp)
			{

				return Json(new { success = false, responseText = exp.Message });
			}

		}




        [HttpPost]

        public IActionResult UpdateSubCategory(SubCategory subCategory)
        {

            try
            {


                //LoadCategoryDDL();
                if (subCategory.image != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(subCategory.image.FileName);
                    var fileExtension = Path.GetExtension(subCategory.image.FileName);
                    var Image = $"{fileName}_{Guid.NewGuid().ToString()}.{fileExtension}";

                    string wwwRootPath = _hostingEnvironment.WebRootPath;
                    string CatName = _categoryRepository.GetCategoryById(subCategory.CategoryId).CategoryName;
                    string UploadedFolder = $"/Uploadimages/subcategory/{CatName}/";



                    var basePath = Path.Combine(wwwRootPath + UploadedFolder);



                    bool basePathExists = System.IO.Directory.Exists(basePath);


                    if (!basePathExists) Directory.CreateDirectory(basePath);



                    var filePath = Path.Combine(basePath, Image);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        subCategory.image.CopyTo(stream);


                    }
                    subCategory.ImageUrl = UploadedFolder + Image;

                }

               

                bool updateSubCat = _subCategoryDAL.UpdateSubCategory(subCategory);

                return Json(new { code = true, jsonText = "SubCategory Updated  Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }



        public JsonResult DeleteSubCategory(int id)
        {
            try
            {



                bool res = _subCategoryDAL.DeleteSubCategory(id, -1);

                return Json(new { success = true, responseText = "SubCategory Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }
    }
}
