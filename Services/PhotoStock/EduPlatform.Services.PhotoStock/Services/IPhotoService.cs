using EduPlatform.Services.PhotoStock.Dtos;
using EduPlatform.Shared.Dtos;
namespace EduPlatform.Services.PhotoStock.Services
{
	public interface IPhotoService
	{
		Task<ResponseDto<ResponsePhotoDto>> SavePhotoAsync(IFormFile imageFile, CancellationToken cancellationToken);
		ResponseDto<NoContent> DeletePhoto(string imageUrl);
	}
}
