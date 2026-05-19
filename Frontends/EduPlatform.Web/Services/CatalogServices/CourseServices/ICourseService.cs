using EduPlatform.Web.Models.CatalogViewModels;

namespace EduPlatform.Web.Services.CatalogServices.CourseServices
{
	public interface ICourseService
	{
		Task<IEnumerable<CourseViewModel>> GetAllAsync();
		Task<IEnumerable<CourseViewModel>> GetAllByUserIdAsync(string userId);
		Task<CourseViewModel> GetByIdAsync(string id);
		Task<bool> CreateAsync(CreateCourseRequest p);
		Task<bool> UpdateAsync(UpdateCourseRequest p);
		Task<bool> DeleteAsync(string id);
	}
}
