namespace EduPlatform.Web.Models.OrderViewModels
{
	public class OrderCreatedViewModel
	{
		//sipariş başarılı oluşursa o siparişe ait id dönsün bana.
		public int? OrderId { get; set; }
		//Ödeme alınamazsa
		public string? ErrorMessage { get; set; }
		public bool IsSuccessFul { get; set; }
	}
}
