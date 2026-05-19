using EduPlatform.Web.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Web.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		public UserController(IUserService userService)
		{
			_userService = userService;
		}
		public async Task<IActionResult> Index()
		{
			var response = await _userService.GetUser();
			return View(response);
		}
	}
}
