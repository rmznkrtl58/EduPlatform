using EduPlatform.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Services.Catalog.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomBaseController:ControllerBase
	{
		public IActionResult CreateActionResultInstance<T>(ResponseDto<T> response)
		{
			return new ObjectResult(response)
			{
				StatusCode = response.StatusCode
			};
		}
	}
}
