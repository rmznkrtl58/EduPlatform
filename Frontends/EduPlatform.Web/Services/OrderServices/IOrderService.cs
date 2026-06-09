using EduPlatform.Web.Models.OrderViewModels;

namespace EduPlatform.Web.Services.OrderServices
{
	public interface IOrderService
	{
		//Senkron yol
		Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoRequest p);
		//asenkron yol->ödemeyi alıp rabbitmq'ye ileteceğiz.
		//artık sonucunu beklemeyeceğiz orda sipariş oluşacak.
		//rabbitmqye göndereceğiz oda kendi mikroservicinde yani order
		//servicede siparişi oluşturacak.
		Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoRequest p);
		//Satın alma geçmişindeki bütün siparişleri aldığım metodum
		Task<List<OrderViewModel>> GetOrder();
	}
}
