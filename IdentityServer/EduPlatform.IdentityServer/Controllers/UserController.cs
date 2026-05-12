using EduPlatform.IdentityServer.Dtos;
using EduPlatform.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;
namespace EduPlatform.IdentityServer.Controllers
{
	[Authorize(LocalApi.PolicyName)]
	public class UserController : CustomBaseController
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		public async Task<IActionResult> CreateUser(CreateUserDto p)
		{
			return CreateActionResultInstance(await _userService.CreateUserAsync(p));
		}
		[HttpGet]
		public async Task<IActionResult> GetUserInfo()
		{
			var response= await _userService.GetUserInformationAsync(HttpContext);
			return CreateActionResultInstance(response);
		}
	}
}

