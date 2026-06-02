using EduPlatform.Services.Basket.Dtos;
using EduPlatform.Services.Basket.Services;
using EduPlatform.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Services.Basket.Controllers
{
	public class BasketsController : CustomBaseController
	{
		private readonly IBasketService _basketService;
		private readonly ISharedIdentityService _identityService;
		public BasketsController(IBasketService basketService, ISharedIdentityService identityService)
		{
			_basketService = basketService;
			_identityService = identityService;
		}
		[HttpGet]
		public async Task<IActionResult>GetBasket()
		{
			var response = await _basketService.GetBasketByUser(_identityService.GetUserId);
			return CreateActionResultInstance(response);
		}
		[HttpPost]
		public async Task<IActionResult> SaveOrUpdate(BasketDto p)
		{
			p.UserId = _identityService.GetUserId;
			var response = await _basketService.SaveOrUpdate(p);
			return CreateActionResultInstance(response);
		}
		[HttpDelete]
		public async Task<IActionResult> Delete()
		{
			var response = await _basketService.Delete(_identityService.GetUserId);
			return CreateActionResultInstance(response);
		}
	}
}
