using EduPlatform.Web.Exceptions;
using EduPlatform.Web.Models.Other;
using EduPlatform.Web.Services.CatalogServices.CourseServices;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EduPlatform.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICourseService _courseService;
		public HomeController(ILogger<HomeController> logger, ICourseService courseService)
		{
			_logger = logger;
			_courseService = courseService;
		}

		public async Task<IActionResult> Index()
		{
			var response = await _courseService.GetAllAsync();
			return View(response);
		}
		public async Task<IActionResult> Detail(string id)
		{
			var response = await _courseService.GetByIdAsync(id);
			return View(response);
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			//İlgili exceptionuma ait yapıları getir diyorum.ilgili custom exceptionum zaten exceptiondan
			//miras aldığı için direk yapıları alabilirim
			var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
			if (errorFeature is not null && errorFeature.Error is UnAuthorizeException)
				return RedirectToAction(nameof(AuthController.LogOut), "Auth");
			
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
