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
			var orderStatus = await _orderService.CreateOrder(p);
			if (!orderStatus.IsSuccessFul)
			{
				var basket = await _basketService.GetBasket();
				ViewBag.basket = basket;
				ViewBag.error = orderStatus.ErrorMessage;
				return View();
			}

			//eğerki ödeme başarılıysa ilgili sayfaya yönlendir.
			return RedirectToAction("SuccessfulCheckout", new { orderId=orderStatus.OrderId});
		}
		public async Task<IActionResult> SuccessfulCheckout(int orderId)
		{
			ViewBag.orderId = orderId;
			return View();
		}

	}
}
