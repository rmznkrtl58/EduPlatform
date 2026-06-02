using EduPlatform.Web.Models.BasketViewModels;

namespace EduPlatform.Web.Services.BasketServices
{
	public interface IBasketService
	{
		Task<bool> SaveOrUpdate(BasketViewModel p);
		Task<BasketViewModel> GetBasket();
		Task<bool> Delete();
		Task AddBasketItem(BasketItemViewModel p);
		Task<bool> RemoveBasketItem(string courseId);
		Task<bool> ApplyDiscount(string discountCode);
		Task<bool> CancelApplyDiscount();
	}
}
