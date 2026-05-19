using EduPlatform.Shared.Services;
using EduPlatform.Web.Services.CatalogServices.CourseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Web.Controllers
{
	[Authorize]
	public class CourseController : Controller
    {
		private readonly ICourseService _coursService;
		private readonly ISharedIdentityService _sharedIdentityService;
		public CourseController(ICourseService coursService, ISharedIdentityService sharedIdentityService)
		{
			_coursService = coursService;
			_sharedIdentityService = sharedIdentityService;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var response = await _coursService.GetAllByUserIdAsync(_sharedIdentityService.GetUserId);
			return View(response);
		}
	}
}
