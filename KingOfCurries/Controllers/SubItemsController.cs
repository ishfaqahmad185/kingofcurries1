using _Helper;
using KingofCurries.Models;
using HarrysRestro.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KingofCurries.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubItemsController : Controller
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private ISubItemsRepository _subItemsRepository;
        public SubItemsController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,ISubItemsRepository subItemsRepository)
        {

            _environment = environment;
            _subItemsRepository = subItemsRepository;

        }
        public IActionResult Index()
        {
            LoadItemDDL();
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> AddItems(SubItems subItems)
        {
            try
            {
                var path = _environment.WebRootPath + @"\Uploadimages\SubItems\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string image1 = null, image2 = null;

                string uploadsFolder = Path.Combine(_environment.WebRootPath, "Uploadimages/SubItems");
                image1 = Guid.NewGuid().ToString() + "_" + subItems.SubItemImageUrl.FileName;
                image2 = Guid.NewGuid().ToString() + "_" + subItems.SubItemImageUrl.FileName;
                string filePath1 = Path.Combine(uploadsFolder, image1);
                string filePath2 = Path.Combine(uploadsFolder, image2);
                using (var fileStream = new FileStream(filePath1, FileMode.Create))
                {
                    subItems.SubItemImageUrl.CopyTo(fileStream);
                }
                using (var fileStream = new FileStream(filePath2, FileMode.Create))
                {
                    subItems.SubItemImageUrl.CopyTo(fileStream);
                }

                var files = UploadProductImagesAsync(subItems.SubItemImageUrl, subItems.ItemId);
                subItems.SubItemImage = files[0];
                subItems.SubItemThumbnail = files[1];
                subItems.Slug = $"{subItems.SubItemTitle.Replace(" ", "-")}-{Guid.NewGuid()}";

                bool check = _subItemsRepository.InsertSubItems(subItems);





                return Json(new { code = true, jsonText = "Added Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }
        private List<string> UploadProductImagesAsync(IFormFile UploadedFiles, int productId)
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




            string imageURL = UploadedFolder + Image;
            string imageThubURL = UploadedFolderThub + Image;

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

        public JsonResult ListSubItems()
        {
            try
            {
                List<SubItems> SubItems = _subItemsRepository.GetAllSubItems().ToList();

                return Json(new { code = true, jsonText = SubItems });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }


        public JsonResult DeleteSubItems(int id)
        {
            try
            {



                bool res = _subItemsRepository.DeleteSubItem(id, -1);
                return Json(new { success = true, responseText = "Deleted Successfully" });
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }



        public IActionResult GetSubItemsById(int id)
        {
            LoadItemDDL();

            try
            {




                SubItems res = _subItemsRepository.GetAllSubItemsById(id);



                return PartialView("_EditSubItems", res); ;
            }
            catch (Exception exp)
            {

                return Json(new { success = false, responseText = exp.Message });
            }

        }

        public IActionResult UpdateSubItems(SubItems subItems)
        {

            try
            {
                subItems.UserId = -1;
                if (subItems.SubItemImageUrl != null)
                {


                    var files = UploadProductImagesAsync(subItems.SubItemImageUrl, subItems.ItemId);
                    subItems.SubItemImage = files[0];
                    subItems.SubItemThumbnail = files[1];


                    bool check = _subItemsRepository.UpdateSubItems(subItems);





                }
                else
                {

                    bool check = _subItemsRepository.UpdateSubItems(subItems);
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

            IEnumerable<Item> items = _subItemsRepository.GetAllItems();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in items)
            {
                selectListItems.Add(new SelectListItem { Text = item.ItemTitle.ToString(), Value = item.ItemId.ToString() });
            }
            ViewBag.Items = selectListItems;

        }
    }
}
