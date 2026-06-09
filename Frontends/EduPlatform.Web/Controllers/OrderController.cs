using EduPlatform.Web.Models.OrderViewModels;
using EduPlatform.Web.Services.BasketServices;
using EduPlatform.Web.Services.OrderServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduPlatform.Web.Controllers
{
	public class OrderController : Controller
	{
		private readonly IBasketService _basketService;
		private readonly IOrderService _orderService;

		public OrderController(IBasketService basketService, IOrderService orderService)
		{
			_basketService = basketService;
			_orderService = orderService;
		}
		[HttpGet]
		public async Task<IActionResult> Checkout()
		{
			var basket = await _basketService.GetBasket();
			ViewBag.basket = basket;
			return View(new CheckoutInfoRequest());
		}
		[HttpPost]
		public async Task<IActionResult> Checkout(CheckoutInfoRequest p)
		{
			//1.yol senkron communication
			//var orderStatus = await _orderService.CreateOrder(p);
			
			//2.yol asenkron communication RabbitMq'lü kullanım
			var orderSuspendStatus = await _orderService.SuspendOrder(p);
			if (!orderSuspendStatus.IsSuccessFul)
			{
				var basket = await _basketService.GetBasket();
				ViewBag.basket = basket;
				ViewBag.error = orderSuspendStatus.ErrorMessage;
				return View();
			}

			//eğerki ödeme başarılıysa ilgili sayfaya yönlendir.
			//return RedirectToAction("SuccessfulCheckout", new { orderId= orderStatus.OrderId});
			return RedirectToAction("SuccessfulCheckout", new { orderId= new Random().Next(1,1000)});
		}
		public IActionResult SuccessfulCheckout(int orderId)
		{
			ViewBag.orderId = orderId;
			return View();
		}
		public async Task<IActionResult> CheckoutHistory()
		{
			return View(await _orderService.GetOrder());
		}

	}
}
