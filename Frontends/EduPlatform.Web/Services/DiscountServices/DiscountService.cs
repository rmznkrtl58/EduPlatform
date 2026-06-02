using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Models.DiscountViewModels;

namespace EduPlatform.Web.Services.DiscountServices
{
	public class DiscountService : IDiscountService
	{
		private readonly HttpClient _httpClient;
		public DiscountService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		//
		public async Task<DiscountViewModel> GetDiscount(string discountCode)
		{
			var response = await _httpClient.GetAsync($"discounts/GetByCoupenCode/{discountCode}");
			if (!response.IsSuccessStatusCode) return null;
			var discount = await response.Content.ReadFromJsonAsync<ResponseDto<DiscountViewModel>>();
			return discount.Data;
		}
	}
}
