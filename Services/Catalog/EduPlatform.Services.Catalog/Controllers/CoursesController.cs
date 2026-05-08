using EduPlatform.Services.Catalog.Dtos.CourseDtos;
using EduPlatform.Services.Catalog.Services;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Services.Catalog.Controllers
{
	public class CoursesController : CustomBaseController
	{
		private readonly ICourseService _courseService;
		public CoursesController(ICourseService courseService)
		{
			_courseService = courseService;
		}
		[HttpGet("{id}")]
		public async Task<IActionResult>GetById(string id)
		{
			var response = await _courseService.GetByIdAsync(id);
			return CreateActionResultInstance(response);
		}
		[HttpGet]
		public async Task<IActionResult> GetAll(string id)
		{
			var response = await _courseService.GetListAllAsync();
			return CreateActionResultInstance(response);
		}
		[HttpGet("GetAllByUserId/{userId}")]
		public async Task<IActionResult> GetAllByUserId(string userId)
		{
			var response = await _courseService.GetListByUserId(userId);
			return CreateActionResultInstance(response);
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateCouseDto p)
		{
			var response = await _courseService.CreateAsync(p);
			return CreateActionResultInstance(response);
		}
		[HttpPut]
		public async Task<IActionResult> Update(UpdateCourseDto p)
		{
			var response = await _courseService.UpdateAsync(p);
			return CreateActionResultInstance(response);
		}
		[HttpDelete("id")]
		public async Task<IActionResult> Delete(string id)
		{
			var response = await _courseService.DeleteAsync(id);
			return CreateActionResultInstance(response);
		}

	}
}
