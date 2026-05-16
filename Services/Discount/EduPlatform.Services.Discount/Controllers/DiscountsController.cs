using EduPlatform.Services.Discount.Dtos;
using EduPlatform.Services.Discount.Services;
using EduPlatform.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Services.Discount.Controllers
{
	public class DiscountsController : CustomBaseController
	{
		private readonly IDiscountService _dService;
		private readonly ISharedIdentityService _identityService;
		public DiscountsController(IDiscountService dService, ISharedIdentityService identityService)
		{
			_dService = dService;
			_identityService = identityService;
		}
		[HttpGet]
		public async Task<IActionResult> GetListAll()
		{
			var response = await _dService.GetListAll();
			return CreateActionResultInstance(response);
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var response = await _dService.GetById(id);
			return CreateActionResultInstance(response);
		}
		[HttpGet("GetByCoupenCode/{code}")]
		public async Task<IActionResult> GetByCoupenCode(string code)
		{
			var response = await _dService.GetDiscountByCodeAndUserId(code,_identityService.GetUserId);
			return CreateActionResultInstance(response);
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateDiscountDto p)
		{
			var response = await _dService.Create(p);
			return CreateActionResultInstance(response);
		}
		[HttpPut]
		public async Task<IActionResult> Update(UpdateDiscountDto p)
		{
			var response = await _dService.Update(p);
			return CreateActionResultInstance(response);
		}
		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var response = await _dService.Delete(id);
			return CreateActionResultInstance(response);
		}
	}
}
