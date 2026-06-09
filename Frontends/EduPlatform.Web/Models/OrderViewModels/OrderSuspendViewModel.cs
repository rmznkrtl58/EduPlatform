namespace EduPlatform.Web.Models.OrderViewModels
{
	public class OrderSuspendViewModel
	{
		//Ödeme alınamazsa
		public string? ErrorMessage { get; set; }
		public bool IsSuccessFul { get; set; }
	}
}
