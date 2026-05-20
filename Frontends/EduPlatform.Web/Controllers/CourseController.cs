using EduPlatform.Shared.Services;
using EduPlatform.Web.Models.CatalogViewModels;
using EduPlatform.Web.Services.CatalogServices.CategoryServices;
using EduPlatform.Web.Services.CatalogServices.CourseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace EduPlatform.Web.Controllers
{
	[Authorize]
	public class CourseController : Controller
    {
		private readonly ICourseService _coursService;
		private readonly ICategoryService _categoryService;
		private readonly ISharedIdentityService _sharedIdentityService;
		public CourseController(ICourseService coursService, ISharedIdentityService sharedIdentityService, ICategoryService categoryService)
		{
			_coursService = coursService;
			_sharedIdentityService = sharedIdentityService;
			_categoryService = categoryService;
		}
		//DropDownMetod
		private async Task<List<SelectListItem>> CategoriesDropDown()
		{
			var categories = await _categoryService.GetListAllAsync();
		    var categoryListItem=new List<SelectListItem>(from x in categories
									 select new SelectListItem
									 {
										 Text = x.Name,
										 Value = x.Id
									 }).ToList();
			return categoryListItem;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var response = await _coursService.GetAllByUserIdAsync(_sharedIdentityService.GetUserId);
			return View(response);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{

			ViewBag.categoryList =await CategoriesDropDown();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateCourseRequest p)
		{
			ViewBag.categoryList = await CategoriesDropDown();
			if (!ModelState.IsValid)
			{
				return View();
			}
			p.UserId = _sharedIdentityService.GetUserId;
			p.Picture = "resim gelecek birazdan bir ayar çekeceğim";
			await _coursService.CreateAsync(p);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public async Task<IActionResult> Update(string id)
		{


			var findCourse = await _coursService.GetByIdAsync(id);
			if (findCourse is null) return RedirectToAction("Index");//hata fırlatılacak
			var updateCourseRequest = new UpdateCourseRequest()
			{
				CategoryId = findCourse.CategoryId,
				Description = findCourse.Description,
				Feature = findCourse.Feature,
				Id = findCourse.Id,
				Name = findCourse.Name,
				Picture = findCourse.Picture,
				Price = findCourse.Price,
				UserId = findCourse.UserId
			};
			ViewBag.categoryList = await CategoriesDropDown();
			return View(updateCourseRequest);
		}
		[HttpPost]
		public async Task<IActionResult> Update(UpdateCourseRequest p)
		{
			ViewBag.categoryList = await CategoriesDropDown();
			if (!ModelState.IsValid) return View();
			await _coursService.UpdateAsync(p);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(string id)
		{
			await _coursService.DeleteAsync(id);
			return RedirectToAction("Index");
		}
	}
}
