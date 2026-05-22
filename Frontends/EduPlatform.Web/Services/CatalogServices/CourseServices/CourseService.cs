using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Helpers;
using EduPlatform.Web.Models.CatalogViewModels;
using EduPlatform.Web.Services.PhotoStockServices;
namespace EduPlatform.Web.Services.CatalogServices.CourseServices
{
	public class CourseService : ICourseService
	{
		private readonly HttpClient _httpClient;
		private readonly IPhotoStockService _photoService;
		private readonly PhotoHelper _photoHelper;
		public CourseService(HttpClient httpClient, IPhotoStockService photoService, PhotoHelper photoHelper)
		{
			_httpClient = httpClient;
			_photoService = photoService;
			_photoHelper = photoHelper;
		}

		public async Task<bool> CreateAsync(CreateCourseRequest p)
		{
			//Kurs Fotoğraf işlemi
			var responseImageResult = await _photoService.UploadPhoto(p.PhotoFormFile);
			if (responseImageResult is not null) p.Picture = responseImageResult.Url;



			var response = await _httpClient.PostAsJsonAsync<CreateCourseRequest>("courses", p);
			return response.IsSuccessStatusCode;//zaten başarısız ise false dönecek.
		}

		public async Task<bool> DeleteAsync(string id)
		{
			var response = await _httpClient.DeleteAsync($"courses/{id}");
			return response.IsSuccessStatusCode;
		}

		public async Task<IEnumerable<CourseViewModel>> GetAllAsync()
		{
			var response = await _httpClient.GetAsync("courses");
			if (!response.IsSuccessStatusCode) return null;
			var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<CourseViewModel>>>();
			foreach (var course in content!.Data) 
			{
				course.StockPictureUrl = _photoHelper.GetPhotoStockUrl(course.Picture);
			}
			return content.Data;
		}

		public async Task<IEnumerable<CourseViewModel>> GetAllByUserIdAsync(string userId)
		{
			//[HttpGet("GetAllByUserId/{userId}")]
			var response = await _httpClient.GetAsync($"courses/GetAllByUserId/{userId}");
			if (!response.IsSuccessStatusCode) return null;
			var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<CourseViewModel>>>();
			foreach (var course in content!.Data)
			{
				course.StockPictureUrl = _photoHelper.GetPhotoStockUrl(course.Picture);
			}
			return content.Data;
		}

		public async Task<CourseViewModel> GetByIdAsync(string id)
		{
			//[HttpGet("GetAllByUserId/{userId}")]
			var response = await _httpClient.GetAsync($"courses/{id}");
			if (!response.IsSuccessStatusCode) return null;
			var content = await response.Content.ReadFromJsonAsync<ResponseDto<CourseViewModel>>();
			content!.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(content.Data.Picture);
			return content.Data;
		}

		public async Task<bool> UpdateAsync(UpdateCourseRequest p)
		{
			//Kurs Fotoğraf işlemi
			var responseImageResult = await _photoService.UploadPhoto(p.PhotoFormFile);
			if (responseImageResult is not null)
			{
				await _photoService.DeletePhoto(p.Picture);
				p.Picture = responseImageResult.Url;
			}
				


			var response = await _httpClient.PutAsJsonAsync<UpdateCourseRequest>("courses", p);
			return response.IsSuccessStatusCode;//zaten başarısız ise false dönecek.
		}
	}
}
