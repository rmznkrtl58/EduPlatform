using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Models.CatalogViewModels;
namespace EduPlatform.Web.Services.CatalogServices.CourseServices
{
	public class CourseService : ICourseService
	{
		private readonly HttpClient _httpClient;
		public CourseService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<bool> CreateAsync(CreateCourseRequest p)
		{
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
			return content.Data;

		}

		public async Task<IEnumerable<CourseViewModel>> GetAllByUserIdAsync(string userId)
		{
			//[HttpGet("GetAllByUserId/{userId}")]
			var response = await _httpClient.GetAsync($"courses/GetAllByUserId/{userId}");
			if (!response.IsSuccessStatusCode) return null;
			var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<CourseViewModel>>>();
			return content.Data;
		}

		public async Task<CourseViewModel> GetByIdAsync(string id)
		{
			//[HttpGet("GetAllByUserId/{userId}")]
			var response = await _httpClient.GetAsync($"courses/{id}");
			if (!response.IsSuccessStatusCode) return null;
			var content = await response.Content.ReadFromJsonAsync<ResponseDto<CourseViewModel>>();
			return content.Data;
		}

		public async Task<bool> UpdateAsync(UpdateCourseRequest p)
		{
			var response = await _httpClient.PostAsJsonAsync<UpdateCourseRequest>("courses", p);
			return response.IsSuccessStatusCode;//zaten başarısız ise false dönecek.
		}
	}
}
