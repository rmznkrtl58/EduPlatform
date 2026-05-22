using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Models.PhotoStockModels;

namespace EduPlatform.Web.Services.PhotoStockServices
{
	public class PhotoStockService : IPhotoStockService
	{
		private readonly HttpClient _httpClient;
		public PhotoStockService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<bool> DeletePhoto(string imageUrl)
		{
			var response=await _httpClient.DeleteAsync($"photos?imageUrl={imageUrl}");
			return response.IsSuccessStatusCode;
		}

		public async Task<PhotoStockViewModel> UploadPhoto(IFormFile imageFile)
		{
			if (imageFile is null && imageFile.Length >= 0) return null;
			//1532463462462.jpg gibi bir şey gelecekş
			var imageFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(imageFile.FileName)}";
			using (var ms=new MemoryStream())
			{
				//şimdilik memoryde oluşacak dosyam
				await imageFile.CopyToAsync(ms);
				var multiPartContent = new MultipartFormDataContent();
				//burdaki ikinci overload önemli biz photoStock apide fotoyu yükleme endpointinde
				//parametre olarak IFormFile imageFile diye geçtiğimiz için orayada onu yazdık.
				multiPartContent.Add(new ByteArrayContent(ms.ToArray()), "imageFile", imageFileName);
				var response = await _httpClient.PostAsync("photos", multiPartContent);
				if (response.IsSuccessStatusCode) return null;//loglama geçilecekse yazılır buraya.
				return (await response.Content.ReadFromJsonAsync<ResponseDto<PhotoStockViewModel>>()).Data;
			}
		}
	}
}
