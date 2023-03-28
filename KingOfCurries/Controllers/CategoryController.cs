using _Helper;
using KingofCurries.Models;

using Microsoft.AspNetCore.Mvc;
using Repository;
using Services;

namespace KingofCurries.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CategoryController(ICategoryRepository categoryRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
           _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]

        public IActionResult AddCategory(Category category)
        {
            try
            {
                 

                 
                var fileName = Path.GetFileNameWithoutExtension(category.Image.FileName);
                var fileExtension = Path.GetExtension(category.Image.FileName);
                var Image = $"{fileName}_{Guid.NewGuid().ToString()}.{fileExtension}";

                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string UploadedFolder = $"/Uploadimages/CategoryImages/";




                var basePath = Path.Combine(wwwRootPath + UploadedFolder);



                bool basePathExists = System.IO.Directory.Exists(basePath);



                if (!basePathExists) Directory.CreateDirectory(basePath);



                var filePath = Path.Combine(basePath, Image);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    category.Image.CopyTo(stream);


                }

                string imageURL = UploadedFolder + Image;


                category.UserId = -1;
                category.CategoryImageurl = imageURL;
                category.Slug = category.CategoryName.Replace(" ", "-");

                bool check = _categoryRepository.Addcategory(category);

                 
                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }

        }

        public JsonResult ListCategoryGetAll()
        {
            try
            {


                List<Category> category = _categoryRepository.GetAllCategory().ToList();

                return Json(new { code = true, jsonText = category });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }



        public IActionResult EditCategoryById(int id)
        {
            try
            {

                Category edit = _categoryRepository.GetCategoryById(id);
                return PartialView("_EditCategory", edit); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }
        }


            [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            try
            {
                if (category.Image != null)
                {
                    var fileName = Path.GetFileNameWithoutExtension(category.Image.FileName);
                    var fileExtension = Path.GetExtension(category.Image.FileName);
                    var Image = $"{fileName}_{Guid.NewGuid().ToString()}.{fileExtension}";

                    string wwwRootPath = _hostingEnvironment.WebRootPath;
                    string UploadedFolder = $"/Uploadimages/CategoryImages/";




                    var basePath = Path.Combine(wwwRootPath + UploadedFolder);



                    bool basePathExists = System.IO.Directory.Exists(basePath);



                    if (!basePathExists) Directory.CreateDirectory(basePath);



                    var filePath = Path.Combine(basePath, Image);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        category.Image.CopyTo(stream);


                    }

                    string imageURL = UploadedFolder + Image;

                    category.CategoryImageurl = imageURL;

                }
                category.UserId = -1;

                bool updateCat = _categoryRepository.Updatecategory(category);



                return Json(new { code = true, jsonText = "Category Updated Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }

        }


        public JsonResult DeleteCategory(int id)
        {
            try
            {



                bool res = _categoryRepository.DeleteCategory(id, -1);

                return Json(new { success = true, responseText = "Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

    }
}
