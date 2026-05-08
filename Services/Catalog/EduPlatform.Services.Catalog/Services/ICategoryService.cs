using EduPlatform.Services.Catalog.Dtos.CategoryDtos;
using EduPlatform.Shared.Dtos;

namespace EduPlatform.Services.Catalog.Services
{
	public interface ICategoryService
	{
		Task<ResponseDto<IEnumerable<GetCategoryDto>>> GetListAllAsync();
		Task<ResponseDto<GetCategoryDto>> GetByIdAsync(string id);
		Task<ResponseDto<CreateCategoryDto>> CreateAsync(CreateCategoryDto p);
	}
}
