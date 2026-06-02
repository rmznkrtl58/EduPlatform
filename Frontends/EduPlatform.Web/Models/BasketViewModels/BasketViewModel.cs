namespace EduPlatform.Web.Models.BasketViewModels
{
	public class BasketViewModel
	{
		public BasketViewModel()
		{
			_BasketItems = new List<BasketItemViewModel>();
		}
		public string UserId { get; set; }
		public string? DiscountCode { get; set; }
		public int? DiscountRate { get; set; }
		private List<BasketItemViewModel> _BasketItems;
		public decimal TotalPrice
		{
			get => _BasketItems.Sum(x =>x.CurrentPrice);
		}
		//Eğerki indirim kodu varsa true dönsün yoksa false olsun.
		public bool HasDiscount=>!string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
		//Sepet ürünlerini durumlara göre getirme metodu
		public List<BasketItemViewModel> BasketItems
		{
			get
			{
				if (HasDiscount)//eğerki indirim kuponu varsa
				{
					_BasketItems.ForEach(x =>
					{
						var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
						x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
					});
				}
				return _BasketItems;
			}

			set
			{
              _BasketItems= value;
			}

		}
		
	}
}
