using EduPlatform.Web.Models.PhotoStockModels;

namespace EduPlatform.Web.Services.PhotoStockServices
{
	public interface IPhotoStockService
	{
		Task<PhotoStockViewModel> UploadPhoto(IFormFile file);
		Task<bool> DeletePhoto(string photoUrl);
	}
}
