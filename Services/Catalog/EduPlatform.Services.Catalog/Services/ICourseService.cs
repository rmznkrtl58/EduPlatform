using EduPlatform.Services.Catalog.Dtos.CourseDtos;
using EduPlatform.Shared.Dtos;

namespace EduPlatform.Services.Catalog.Services
{
	public interface ICourseService
	{
		Task<ResponseDto<IEnumerable<GetCourseDto>>> GetListAllAsync();
		Task<ResponseDto<GetCourseDto>> GetByIdAsync(string id);
		Task<ResponseDto<IEnumerable<GetCourseDto>>> GetListByUserId(string userId);
		Task<ResponseDto<CreateCouseDto>> CreateAsync(CreateCouseDto p);
		Task<ResponseDto<NoContent>> UpdateAsync(UpdateCourseDto p);
		Task<ResponseDto<NoContent>> DeleteAsync(string id);
	}
}
