using EduPlatform.Web.Models.CatalogViewModels;

namespace EduPlatform.Web.Services.CatalogServices.CategoryServices
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryViewModel>> GetListAllAsync();
		Task<CategoryViewModel> GetByIdAsync(string id);
		Task<bool> CreateAsync(CreateCategoryRequest p);
	}
}
