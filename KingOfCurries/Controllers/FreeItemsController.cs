using _Helper;
using KingofCurries.Models;
using HarrysRestro.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KingofCurries.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FreeItemsController : Controller
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private IFreeItemsRepository _freeItemsRepository;
        public FreeItemsController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,IFreeItemsRepository freeItemsRepository)
        {

            _environment = environment;
            _freeItemsRepository = freeItemsRepository;

        }
        public IActionResult Index()
        {
            LoadItemDDL();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItems(FreeItems freeItems ) {
            try
            {
                var path = _environment.WebRootPath + @"\Uploadimages\FreeItems\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string image1=null, image2 = null ;
              
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploadimages/FreeItems");
                    image1 = Guid.NewGuid().ToString() + "_" + freeItems.FreeItemImageUrl.FileName;
                    image2 = Guid.NewGuid().ToString() + "_" + freeItems.FreeItemImageUrl.FileName;
                    string filePath1 = Path.Combine(uploadsFolder, image1);
                    string filePath2 = Path.Combine(uploadsFolder, image2);
                

                var files = UploadProductImagesAsync(freeItems.FreeItemImageUrl, freeItems.ItemId);
                freeItems.FreeItemImage = files[0];
                freeItems.FreeItemThumbnail = files[1];
                freeItems.Slug = $"{freeItems.FreeItemTitle.Replace(" ", "-")}-{Guid.NewGuid()}";
                bool check =  _freeItemsRepository.InsertFreeItems(freeItems);   


 


                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {
                
                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }
        private List<string> UploadProductImagesAsync(IFormFile UploadedFiles, int productId  )
        {

            int res = 0;
            int count = 0;
            

            var fileName = Path.GetFileNameWithoutExtension(UploadedFiles.FileName);
            var fileExtension = Path.GetExtension(UploadedFiles.FileName);
            var Image = $"{fileName}_{Guid.NewGuid().ToString()}.{fileExtension}";

            string wwwRootPath = _environment.WebRootPath;
            string UploadedFolder = $"/Uploads/ProductImages/{productId}/Default/";
            string UploadedFolderThub = $"/Uploads/ProductImages/{productId}/Thubnail/";
          
           

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


            var filePaththub = Path.Combine(basePaththub, Image);
            using (var stream = new FileStream(filePaththub, FileMode.Create))
            {
                stream.Write(imageBytesThumb, 0, imageBytesThumb.Length);
            }

            


            string imageURL =   UploadedFolder + Image;
            string imageThubURL =  UploadedFolderThub + Image;

            var filepaths = new
            {
                FullPath = imageURL,
                imageThubURL = imageThubURL
            };

            List<string> files = new List<string>();
            files.Add(imageURL);
            files.Add(imageThubURL);

            return files;
        }


        [HttpGet]

        public JsonResult ListFreeItems()
        {
            try
            {
                List<FreeItems> freeItems = _freeItemsRepository.GetAllFreeItems().ToList();

                return Json(new { code = true, jsonText = freeItems });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        public JsonResult DeleteFreeItems(int id)
        {
            try
            {



                bool res = _freeItemsRepository.DeleteFreeItem(id, -1);
                return Json(new { success = true, responseText = "Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }


      
        public IActionResult GetFreeItemsById(int id)
        {
            LoadItemDDL();

            try
            {


                

                FreeItems res = _freeItemsRepository.GetAllFreeItemsById(id);



                return PartialView("_EditFreeItems", res); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

        public IActionResult UpdateFreeItems(FreeItems freeItems)
        {

            try
            {
                freeItems.UserId = -1;
                if (freeItems.FreeItemImageUrl != null)
                {


                    var files = UploadProductImagesAsync(  freeItems.FreeItemImageUrl,freeItems.ItemId);
                    freeItems.FreeItemImage = files[0];
                    freeItems.FreeItemThumbnail = files[1];


                    bool check = _freeItemsRepository.UpdateFreeItems(freeItems);


                   


                }
                else
                {

                    bool check = _freeItemsRepository.UpdateFreeItems(freeItems);
                }

                return Json(new { code = true, jsonText = "Updated Successfully" });

            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }
        public void LoadItemDDL()
        {

            IEnumerable<Item> items = _freeItemsRepository.GetAllItems();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in items)
            {
                selectListItems.Add(new SelectListItem { Text = item.ItemTitle.ToString(), Value = item.ItemId.ToString() });
            }
            ViewBag.Items = selectListItems;

        }

    }
}
