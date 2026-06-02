using EduPlatform.Web.Models.BasketViewModels;
using EduPlatform.Web.Models.DiscountViewModels;
using EduPlatform.Web.Services.BasketServices;
using EduPlatform.Web.Services.CatalogServices.CourseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Web.Controllers
{
	[Authorize]
	public class BasketController : Controller
	{
		private readonly IBasketService _basketService;
		private readonly ICourseService _courseService;
		public BasketController(IBasketService basketService, ICourseService courseService)
		{
			_basketService = basketService;
			_courseService = courseService;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var myBaskets = await _basketService.GetBasket();
			return View(myBaskets);
		}
		public async Task<IActionResult> AddBasketItem(string courseId)
		{
			var findCourse = await _courseService.GetByIdAsync(courseId);
			var basketItem = new BasketItemViewModel()
			{
				CourseId = findCourse.Id,
				CourseName = findCourse.Name,
				Price = findCourse.Price,
			};
			await _basketService.AddBasketItem(basketItem);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> RemoveBasketItem(string courseId)
		{
			await _basketService.RemoveBasketItem(courseId);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> ApplyDiscount(DiscountApplyRequest p)
		{
			if (!ModelState.IsValid)
			{
				TempData["discountError"] =
					ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
				return RedirectToAction("Index");
			}

			var discountStatus = await _basketService.ApplyDiscount(p.Code);
			TempData["discountStatus"] = discountStatus;
			
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> CancelApplyDiscount()
		{
			await _basketService.CancelApplyDiscount();
			return RedirectToAction("Index");
		}
	}
}
