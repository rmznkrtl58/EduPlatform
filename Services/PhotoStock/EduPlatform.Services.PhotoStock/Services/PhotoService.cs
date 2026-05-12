using EduPlatform.Services.PhotoStock.Dtos;
using EduPlatform.Shared.Dtos;
using System.Net;

namespace EduPlatform.Services.PhotoStock.Services
{
	public class PhotoService: IPhotoService
	{
		public ResponseDto<NoContent> DeletePhoto(string imageUrl)
		{
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageUrl);
			if (!File.Exists(filePath)) return ResponseDto<NoContent>.Fail("Resim bulunamadı!", HttpStatusCode.NotFound.GetHashCode());
			File.Delete(filePath);
			return ResponseDto<NoContent>.Success(HttpStatusCode.NoContent.GetHashCode());
		}

		public async Task<ResponseDto<ResponsePhotoDto>> SavePhotoAsync(IFormFile imageFile, CancellationToken cancellationToken)
		{
			if (imageFile is null && imageFile.Length < 0) ResponseDto<ResponsePhotoDto>.Fail("image dosyası yoktur.", HttpStatusCode.BadRequest.GetHashCode());
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFile.FileName);
			using (var stream = new FileStream(path, FileMode.Create))
			{
				await imageFile.CopyToAsync(stream, cancellationToken);
			}

			var returnPath = "images/" + imageFile.FileName;
			var responseDto = new ResponsePhotoDto() { Url = returnPath };

			return ResponseDto<ResponsePhotoDto>.Success(responseDto, HttpStatusCode.Created.GetHashCode());
		}
	}
}
