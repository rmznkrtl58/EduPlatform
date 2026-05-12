using EduPlatform.Services.PhotoStock.Services;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Services.PhotoStock.Controllers
{
	public class PhotosController : CustomBaseController
	{
		private readonly IPhotoService _pService;
		public PhotosController(IPhotoService pService)
		{
			_pService = pService;
		}
		[HttpPost]
		public async Task<IActionResult> SavePhoto(IFormFile imageFile,CancellationToken cancellationToken)
		{
			var response = await _pService.SavePhotoAsync(imageFile,cancellationToken);
			return CreateActionResultInstance(response);
		}
		[HttpDelete]
		public IActionResult DeletePhoto(string imageName)
		{
			var response =  _pService.DeletePhoto(imageName);
			return CreateActionResultInstance(response);
		}
	}
}
