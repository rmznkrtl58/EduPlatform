namespace EduPlatform.Web.Models.OrderViewModels
{
	public class CreateOrderRequest
	{
		public CreateOrderRequest()
		{
			OrderItems = new List<OrderItemViewModel>();
		}
		public string BuyerId { get; set; }
		public List<OrderItemViewModel> OrderItems { get; set; }
		public AddressCreateRequest Address { get; set; }
	}
}
