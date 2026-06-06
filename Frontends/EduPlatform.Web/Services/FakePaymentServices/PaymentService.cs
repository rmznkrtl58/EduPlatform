using EduPlatform.Web.Models.FakePaymentViewModels;

namespace EduPlatform.Web.Services.FakePaymentServices
{
	public class PaymentService : IPaymentService
	{
		private readonly HttpClient _httpClient;
		public PaymentService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		async Task<bool> IPaymentService.ReceivePayment(PaymentInfoRequest p)
		{
			var response = await _httpClient.PostAsJsonAsync<PaymentInfoRequest>("fakepayments",p);
			return response.IsSuccessStatusCode;
		}
	}
}
