using EduPlatform.Services.Catalog.Dtos.CategoryDtos;
using EduPlatform.Services.Catalog.Dtos.FeatureDtos;
namespace EduPlatform.Services.Catalog.Dtos.CourseDtos
{
	public class GetCourseDto
	{
		
		public string Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Picture { get; set; }
		public DateTime CreatedDate { get; set; }
		public string UserId { get; set; }
		public GetFeatureDto Feature { get; set; }
		public string CategoryId { get; set; }
		public GetCategoryDto Category { get; set; }
		public string Description { get; set; }
	}
}
