namespace EduPlatform.Web.Models.BasketViewModels
{
	public class BasketItemViewModel
	{
		public string CourseId { get; set; }
		public string CourseName { get; set; }
		public decimal Price { get; set; }
		//bir kurs alma sitesi olarak düşündüğümüz için 1 yolluyorum
		public int Quantity { get; set; }=1;
		private decimal? DiscountAppliedPrice;
		//İndirimli Fiyatı alıyorum
		public void AppliedDiscount(decimal discountPrice)
		{
			DiscountAppliedPrice = discountPrice;
		}
		//Toplam indirimli indirimsiz fiyatı alıyorum belirli durumlara göre 
		public decimal CurrentPrice
			=> DiscountAppliedPrice is not null ? DiscountAppliedPrice.Value
								   : Price;
 	}
}
