using EduPlatform.Web.Models.DiscountViewModels;

namespace EduPlatform.Web.Services.DiscountServices
{
	public interface IDiscountService
	{
		Task<DiscountViewModel> GetDiscount(string discountCode);
	}
}
