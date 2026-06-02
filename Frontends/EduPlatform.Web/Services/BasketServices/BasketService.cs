using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Models.BasketViewModels;
using EduPlatform.Web.Services.DiscountServices;

namespace EduPlatform.Web.Services.BasketServices
{
	public class BasketService : IBasketService
	{
		private readonly HttpClient _httpClient;
		private readonly IDiscountService _discountService;
		public BasketService(HttpClient httpClient, IDiscountService discountService)
		{
			_httpClient = httpClient;
			_discountService = discountService;
		}
		public async Task AddBasketItem(BasketItemViewModel p)
		{
			var getBasket = await GetBasket();
			if(getBasket is not null)
			{
				//elimde bulunan ilgili kullanıcıma ait sepetimin içerisindeki ürünler var mı yok mu?
				//zaten basket apimde userId'yi tokendan veriyorum.
				if (getBasket.BasketItems.Any(x => x.CourseId == p.CourseId))
				{
					getBasket.BasketItems.Add(p);
				}
			
			}
			else
			{
				getBasket = new BasketViewModel();
				getBasket.BasketItems.Add(p);
			}

			await SaveOrUpdate(getBasket);
		
		}

		public async Task<bool> ApplyDiscount(string discountCode)
		{
			await CancelApplyDiscount();
			var findBasket = await GetBasket();
			if (findBasket is null) return false;
			
			var hasDiscount = await _discountService.GetDiscount(discountCode);
			if (hasDiscount is null) return false;

			findBasket.DiscountCode = hasDiscount.Code;
			findBasket.DiscountRate = hasDiscount.Rate;
			await SaveOrUpdate(findBasket);
			return true;

		}

		public async Task<bool> CancelApplyDiscount()
		{
			var findBasket = await GetBasket();
			if (findBasket is null || findBasket.DiscountCode is null) return false;
			
			findBasket.DiscountCode = null;
			findBasket.DiscountRate = null;
			
			await SaveOrUpdate(findBasket);
			return true;
		}

		public async Task<bool> Delete()
		{
			var response = await _httpClient.DeleteAsync("baskets");
			return response.IsSuccessStatusCode;
		}

		public async Task<BasketViewModel> GetBasket()
		{
			var response = await _httpClient.GetAsync("baskets");
			if (!response.IsSuccessStatusCode) return null;

			var content =await response.Content.ReadFromJsonAsync<ResponseDto<BasketViewModel>>();
			return content.Data;
		}

		public async Task<bool> RemoveBasketItem(string courseId)
		{
			var getBasket = await GetBasket();
			if (getBasket is null) return false;

			//Önce ilgili sepetimdeki ürünleri buluyorum
			var findBasketItem =getBasket.BasketItems.FirstOrDefault(s => s.CourseId == courseId);
			if (findBasketItem is null) return false;

			//ilgili sepetimden o ürünleri kaldırıyorum.
			var deleteResult = getBasket.BasketItems.Remove(findBasketItem);
			if (!deleteResult) return false;

			if (!getBasket.BasketItems.Any()) getBasket.DiscountCode = null;

			return await SaveOrUpdate(getBasket);
		}

		public async Task<bool> SaveOrUpdate(BasketViewModel p)
		{
			var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets",p);
			return response.IsSuccessStatusCode;
		}
	}
}
