namespace EduPlatform.Services.Basket.Dtos
{
	public class BasketDto
	{
		public string UserId { get; set; }
		//İndirim olmadığı zamanlar code giremeyeceğimiz için 
		//burayı not required yapıyorum.
		public string? DiscountCode { get; set; }
		public List<BasketItemDto> BasketItems { get; set; }
		public decimal TotalPrice 
		{
			//her bir sepetimdeki ürünün miktarı ile fiyatını çarpıp üst üste toplaya toplaya gelir.
			get => BasketItems.Sum(x => x.Quantity * x.Price);
		}
	}
}
