
using EduPlatform.Web.Models.OrderViewModels;

namespace EduPlatform.Web.Models.FakePaymentViewModels
{
	public class PaymentInfoRequest
	{
		public string CardName { get; set; }
		public string CardNumber { get; set; }
		public DateTime Expiration { get; set; }
		public string CVV { get; set; }
		public decimal TotalPrice { get; set; }
		public CreateOrderRequest Order { get; set; }
	}
}
