using EduPlatform.Services.Catalog.Dtos.CategoryDtos;
using EduPlatform.Services.Catalog.Services;
using Microsoft.AspNetCore.Mvc;
namespace EduPlatform.Services.Catalog.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : CustomBaseController
	{
		private readonly ICategoryService _cService;
		public CategoriesController(ICategoryService cService)
		{
			_cService = cService;
		}
		[HttpGet]
		public async Task<IActionResult> GetListAll()
		{
			var response = await _cService.GetListAllAsync();
			return CreateActionResultInstance(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var response = await _cService.GetByIdAsync(id);
			return CreateActionResultInstance(response);
		}
		[HttpPost]
		public async Task<IActionResult>Create(CreateCategoryDto p)
		{
			var response = await _cService.CreateAsync(p);
			return CreateActionResultInstance(response);
		}
	}
}
