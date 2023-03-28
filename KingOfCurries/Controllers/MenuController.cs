using KingofCurries.Models;
using HarrysRestro.Repository;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Services;

namespace KingofCurries.Controllers
{

    public class MenuController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subcategoryRepository;
        private readonly IItemRepository _itemRepository;

        private readonly ISubItemsRepository _subItemsRepository;
        private readonly IFreeItemsRepository _freeItemsRepository;

		public MenuController(ICategoryRepository categoryRepository, ISubCategoryRepository subcategoryRepository, IItemRepository itemRepository, ISubItemsRepository subItemsRepository, IFreeItemsRepository freeItemsRepository)
		{
			_categoryRepository = categoryRepository;
			_subcategoryRepository = subcategoryRepository;
			_itemRepository = itemRepository;
			_subItemsRepository = subItemsRepository;
			_freeItemsRepository = freeItemsRepository;
		}

		

        [HttpGet]

        [Route("menu/Restaurant")]
        public IActionResult Restaurant()
        {
            GenericModel balObj = new GenericModel();
            balObj.ListSubCategory = _subcategoryRepository.GetAllSubCategory().ToList();

           

            balObj.ListItem = _itemRepository.GetAllItems().ToList();
          
			balObj.ListAllergens = _itemRepository.GetAllAllergens().ToList();

			return View(balObj);
        } 
        
        
        [HttpGet]

        [Route("menu/Takeaway")]
        public IActionResult Takeaway()
        {
            GenericModel balObj = new GenericModel();
            balObj.ListSubCategory = _subcategoryRepository.GetAllSubCategory().Where(x=>x.isTakeAway==true).ToList();
            var arrayList = _subItemsRepository.GetAllSubItems().ToList().Select(x=>x.ItemId).Distinct().ToList();
            var arrayList2 = _freeItemsRepository.GetAllFreeItems().ToList().Select(x=>x.ItemId).Distinct().ToList();

			var ListItem = _itemRepository.GetAllItems().ToList();
             ListItem.Where(c => arrayList.Contains(c.ItemId)).ToList().ForEach(c => c.IsSubItems = true);
             ListItem.Where(c => arrayList2.Contains(c.ItemId)).ToList().ForEach(c => c.IsFreeItems = true);

            balObj.ListItem = ListItem.OrderBy(x=>x.Priority).ToList();
        
            balObj.ListAllergens=_itemRepository.GetAllAllergens().ToList();

            return View(balObj);
        }



        [HttpGet]

        [Route("menu/SingleMenu/{id}")]
        public IActionResult SingleMenu(string id)
        {
            id = id.Replace("||", "/");
            GenericModel balObj=new GenericModel();
            var CatId = _categoryRepository.GetAllCategory().ToList().Find(x => x.Slug.ToLower() == id.ToLower()).CategoryId;
            balObj.ListSubCategory = _subcategoryRepository.GetAllSubCategory().ToList().FindAll(x => x.CategoryId == CatId);
            balObj.ListItem = _itemRepository.GetAllItems().ToList().FindAll(x => x.CategoryId == CatId);
            balObj.CategoryId= CatId;
            balObj.ListAllergens = _itemRepository.GetAllAllergens().ToList();

            return View(balObj);
        }


       [HttpGet]
       [Route("/Menu/GetMainList/{id}")]

       public JsonResult GetMainList(string id)
        {
            try
            {
                List<Item> list = new List<Item>();
                if (id == "All")
                {
                    list= _itemRepository.GetAllItems().ToList().OrderBy(x => Guid.NewGuid()).Take(8).ToList();
                }
                else {
                    
                    list= _itemRepository.GetAllFilterItems(id).ToList();
                }
				var arrayList = _subItemsRepository.GetAllSubItems().ToList().Select(x => x.ItemId).Distinct().ToList();
				var arrayList2 = _freeItemsRepository.GetAllFreeItems().ToList().Select(x => x.ItemId).Distinct().ToList();

				

				list.Where(c => arrayList.Contains(c.ItemId)).ToList().ForEach(c => c.IsSubItems = true);
				list.Where(c => arrayList2.Contains(c.ItemId)).ToList().ForEach(c => c.IsFreeItems = true);

				return Json(new { code = true, jsonText = list });
            }
            catch (Exception exp)
            {

                return Json(new { code = false, jsonText = exp.Message.ToString() });
            }
        }

        [HttpPost]
        public JsonResult GetSubItemsData(int itemId, bool isSubItem,bool isFreeItem)
        {
            try
            {
                GenericSubModel genericSubModel = new GenericSubModel();
                genericSubModel.ProductId=itemId;
                genericSubModel.IsFreeItems= isFreeItem;
                genericSubModel.IsSubItems= isSubItem;
                genericSubModel.Item = _itemRepository.GetAllItemsById(itemId);
                if (isSubItem)
                {
                    genericSubModel.ListSubItems=_subItemsRepository.GetAllSubItems().ToList().FindAll(x=>x.ItemId==itemId);
                }
                if (isFreeItem)
                {
                    genericSubModel.ListFreeItems = _freeItemsRepository.GetAllFreeItems().ToList().FindAll(x => x.ItemId == itemId);
                }

                return Json(new { success = true, jsonText = genericSubModel });




            }
            catch (Exception exp)
            {

                return Json(new { success = false, jsonText = exp.Message.ToString() }); 
            }
        }


    }
}
