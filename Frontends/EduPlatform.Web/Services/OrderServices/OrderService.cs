using EduPlatform.Shared.Dtos;
using EduPlatform.Shared.Services;
using EduPlatform.Web.Models.FakePaymentViewModels;
using EduPlatform.Web.Models.OrderViewModels;
using EduPlatform.Web.Services.BasketServices;
using EduPlatform.Web.Services.FakePaymentServices;

namespace EduPlatform.Web.Services.OrderServices
{
	public class OrderService : IOrderService
	{
		private readonly IPaymentService _paymentService;
		private readonly HttpClient _httpClient;
		private readonly IBasketService _basketService;
		private readonly ISharedIdentityService _identityService;
		public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService identityService)
		{
			_paymentService = paymentService;
			_httpClient = httpClient;
			_basketService = basketService;
			_identityService = identityService;
		}


		//Direk mikroservice istek yapılacak.
		public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoRequest p)
		{
			var findBasket = await _basketService.GetBasket();
			var paymentInfoRequest = new PaymentInfoRequest()
			{
				CardName = p.CardName,
				CardNumber = p.CardNumber,
				CVV = p.CVV,
				Expiration = p.Expiration,
				TotalPrice = findBasket.TotalPrice
			};
			
			var responsePayment = await _paymentService.ReceivePayment(paymentInfoRequest);
			if (!responsePayment) return new OrderCreatedViewModel()
			{
				ErrorMessage = "Ödeme Başarısız Oldu.",
				IsSuccessFul = false
			};
			//Ödeme işlemlerinin oluşumu için gerekli parametreleri eşliyorum
			var orderCreateRequest = new CreateOrderRequest()
			{
				BuyerId = _identityService.GetUserId,
				Address = new AddressCreateRequest()
				{
					AddressDetail = p.AddressDetail,
					District = p.District,
					Province = p.Province,
					Street = p.Street,
					ZipKode = p.ZipKode
				}
			};
			//şimdi ise ilgili kullanıcıma ait sepetteki itemleri alıp eşlicem
			findBasket.BasketItems.ForEach(basketItem =>
			{
				var orderItem = new OrderItemViewModel()
				{
					CourseId = basketItem.CourseId,
					CourseName = basketItem.CourseName,
					Price = basketItem.CurrentPrice,
					PictureUrl="İlerde uygulamaya eklenecektir."
				};
				orderCreateRequest.OrderItems.Add(orderItem);
			});

			var response = await _httpClient.PostAsJsonAsync<CreateOrderRequest>("orders", orderCreateRequest);
			//ödemede sıkıntı varsa
			if (!response.IsSuccessStatusCode)
			{
				return new OrderCreatedViewModel()
				{
					ErrorMessage = "Sipariş Esnasında Hata Meydana Geldi.",
					IsSuccessFul = false
				};
			}

			//ödeme gerçekleşti :D
			var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<ResponseDto<OrderCreatedViewModel>>();

			orderCreatedViewModel.Data.IsSuccessFul = true;
			await _basketService.Delete();
			return orderCreatedViewModel.Data;
		}

		public async Task<List<OrderViewModel>> GetOrder()
		{
			var response = await _httpClient.GetFromJsonAsync<ResponseDto<List<OrderViewModel>>>("orders");
			return response!.Data;
		}

		//sipariş bilgileri rabbitMq'ye gönderilecek.
		public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoRequest p)
		{

			var basket = await _basketService.GetBasket();
			var orderCreateInput = new CreateOrderRequest()
			{
				BuyerId = _identityService.GetUserId,
				Address = new AddressCreateRequest { Province = p.Province, District = p.District, Street = p.Street, AddressDetail = p.AddressDetail, ZipKode = p.ZipKode },
			};

			basket.BasketItems.ForEach(x =>
			{
				var orderItem = new OrderItemViewModel { CourseId = x.CourseId, Price = x.CurrentPrice, PictureUrl = "", CourseName = x.CourseName };
				orderCreateInput.OrderItems.Add(orderItem);
			});

			var paymentInfoInput = new PaymentInfoRequest()
			{
				CardName = p.CardName,
				CardNumber = p.CardNumber,
				Expiration = p.Expiration,
				CVV = p.CVV,
				TotalPrice = basket.TotalPrice,
				Order = orderCreateInput
			};

			var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

			if (!responsePayment)
			{
				return new OrderSuspendViewModel() { ErrorMessage = "Ödeme alınamadı", IsSuccessFul = false };
			}

			await _basketService.Delete();
			return new OrderSuspendViewModel() { IsSuccessFul = true };
		}
	}
}
