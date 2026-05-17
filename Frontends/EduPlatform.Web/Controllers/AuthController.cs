using EduPlatform.Web.Models;
using EduPlatform.Web.Services.IdentityServices;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IIdentityService _identityService;

		public AuthController(IIdentityService identityService)
		{
			_identityService = identityService;
		}

		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInRequestModel p)
		{
			if (!ModelState.IsValid) return View();
			var response = await _identityService.SignIn(p);
			if (!response.IsSuccessFul)
			{
				response.Errors.ForEach(error =>
				{
					ModelState.AddModelError(string.Empty, error);
				});
				return View();
			}
			return RedirectToAction("Index", "Home");
		}
	}
}
