using _Helper;
using KingofCurries.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KingofCurries.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public ItemController(IItemRepository itemRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment, ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository)
        {
            _itemRepository = itemRepository;
            _environment = Environment;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        public IActionResult Index()
        {
            LoadCategoryDDL();
            return View();
        }

        [HttpGet]
        public JsonResult ListItem()

        {
            try
            {
                LoadCategoryDDL();
                List<Item> item  = _itemRepository.GetAllItems().ToList();

                return Json(new { code = true, jsonText = item });

            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }

        [HttpPost]
        public IActionResult AddItem(Item item)

        {

            try
            {
                if(item.CategoryId <= 0)
                {
                    return Json(new { code = false, jsonText = "Kindly Select a Valid Category" });
                }

                if (item.SubCategoryId <= 0)
                {
                    return Json(new { code = false, jsonText = "Kindly Select a Valid Sub Category" });
                }

                if (item.IsDiscount)
                {
                    item.DiscountAmount = item.DiscountAmount;
                }
                else
                {
                    item.DiscountAmount = 0;
                }


                var files = UploadProductImagesAsync(item.CategoryId,item.SubCategoryId,item.IImage);
                item.ItemImage = files[0];
                item.ThumbnailImage = files[1];
                item.Slug = $"{item.ItemTitle.Replace(" ", "-")}-{Guid.NewGuid()}";
                bool check = _itemRepository.InsertItems(item);




                return Json(new { code = true, jsonText = "Added Successfully" });



            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }

        }
        [HttpPost]
        public JsonResult UpdateItem(Item item)
        {
            try
            {


                item.UserId = -1;
                if (item.IImage != null)
                {


                    var files = UploadProductImagesAsync(item.CategoryId, item.SubCategoryId, item.IImage);
                    item.ItemImage = files[0];
                    item.ThumbnailImage = files[1];


                    bool check = _itemRepository.UpdateItem(item);





                }
                else
                {
                    bool check = _itemRepository.UpdateItem(item);
                }






                return Json(new { code = true, jsonText = "Updated Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        private List<string> UploadProductImagesAsync(int CategoryId,int subCategoryId, IFormFile UploadedFiles)
        {

            int res = 0;
            int count = 0;
          
                var fileName = Path.GetFileNameWithoutExtension(UploadedFiles.FileName);
                var fileExtension = Path.GetExtension(UploadedFiles.FileName);
                var Image = $"{fileName}_{Guid.NewGuid()}{fileExtension}";
                var Image2 = $"{fileName}_{Guid.NewGuid()}{fileExtension}";


                string wwwRootPath = _environment.WebRootPath;
            var Catname = _categoryRepository.GetCategoryById(CategoryId).CategoryName;
            var listData = _subCategoryRepository.GetAllSubCategoryDDL(CategoryId).ToList();
          var subCategoryname=listData.Find(x => x.SubCategoryId == subCategoryId).SubCategoryName;
                string UploadedFolder = $"/Uploads/ProductImages/{Catname}/{subCategoryname}/Default/";
                string UploadedFolderThub = $"/Uploads/ProductImages/{Catname}/{subCategoryname}/Thubnail/";
               
              
                
                var basePath = Path.Combine(wwwRootPath + UploadedFolder);
                var basePaththub = Path.Combine(wwwRootPath + UploadedFolderThub);
   

                bool basePathExists = System.IO.Directory.Exists(basePath);
                bool basePathThubExists = System.IO.Directory.Exists(basePaththub);
 

                if (!basePathExists) Directory.CreateDirectory(basePath);
                if (!basePathThubExists) Directory.CreateDirectory(basePaththub);



                var filePath = Path.Combine(basePath, Image);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    UploadedFiles.CopyTo(stream);


                }
                var imageBytesThumb = Settings.ResizePic(UploadedFiles, 109, 122);


                var filePaththub = Path.Combine(basePaththub, Image2);
                using (var stream = new FileStream(filePaththub, FileMode.Create))
                {
                    stream.Write(imageBytesThumb, 0, imageBytesThumb.Length);
                }

               
                string imageURL =  UploadedFolder + Image;
                string imageThubURL =  UploadedFolderThub + Image2;

                 List<string> files = new List<string>();
                 files.Add(imageURL);
                 files.Add(imageThubURL);

            return files;

        }

        public IActionResult EditItemsById(int id)
        {
            try
            {


                LoadCategoryDDL();
              

                Item res = _itemRepository.GetAllItemsById(id);

              

                return PartialView("_EditItem", res); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

        public JsonResult DeleteItems(int id)
        {
            try
            {



                bool res = _itemRepository.DeleteItems(id, -1);
                return Json(new { success = true, responseText = "Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
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
            ViewBag.Categories = selectListItems;

        }

        [HttpGet]
        public JsonResult LoadSubCategoryDDL(int id)
        {
            try
            {
              
                var subCategories = _subCategoryRepository.GetAllSubCategoryDDL(id).ToList();
                return Json(subCategories);

            }
            catch (Exception exp)
            {
                return Json(exp.Message);

            }
        }

    }
}
