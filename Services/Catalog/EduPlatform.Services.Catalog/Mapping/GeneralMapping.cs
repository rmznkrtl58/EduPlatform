using AutoMapper;
using EduPlatform.Services.Catalog.Dtos.CategoryDtos;
using EduPlatform.Services.Catalog.Dtos.CourseDtos;
using EduPlatform.Services.Catalog.Entities;

namespace EduPlatform.Services.Catalog.Mapping
{
	public class GeneralMapping:Profile
	{
		public GeneralMapping()
		{
			//Course
			CreateMap<Course, GetCourseDto>().ReverseMap();
			CreateMap<Course, CreateCouseDto>().ReverseMap();
			CreateMap<Course, UpdateCourseDto>().ReverseMap();
			//Category
			CreateMap<Category, GetCategoryDto>().ReverseMap();
			CreateMap<Category, CreateCategoryDto>().ReverseMap();
			CreateMap<Category, UpdateCategoryDto>().ReverseMap();
			//Feature
			CreateMap<Feature, GetCategoryDto>().ReverseMap();
			//CreateMap<Feature, CreateFeatureDto>().ReverseMap();
			//CreateMap<Feature, UpdateFeatureDto>().ReverseMap();
		}
	}
}
