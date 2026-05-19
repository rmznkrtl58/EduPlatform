using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Models.CatalogViewModels;

namespace EduPlatform.Web.Services.CatalogServices.CategoryServices
{
	public class CategoryService : ICategoryService
	{
		private readonly HttpClient _httpClient;

		public CategoryService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public Task<bool> CreateAsync(CreateCategoryRequest p)
		{
			throw new NotImplementedException();
		}

		public Task<CategoryViewModel> GetByIdAsync(string id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<CategoryViewModel>> GetListAllAsync()
		{
			var response = await _httpClient.GetAsync("categories");
			if (!response.IsSuccessStatusCode) return null;
			var content = await response.Content.ReadFromJsonAsync<ResponseDto<IEnumerable<CategoryViewModel>>>();
			return content.Data;
		}
	}
}
