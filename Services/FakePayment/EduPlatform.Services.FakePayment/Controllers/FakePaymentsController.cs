using EduPlatform.Services.FakePayment.Models;
using EduPlatform.Services.FakePayment.Services;
using EduPlatform.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace EduPlatform.Services.FakePayment.Controllers
{
	public class FakePaymentsController : CustomBaseController
	{
		private readonly FakePaymentService _paymentService;
		public FakePaymentsController(FakePaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpPost]
		public async Task<IActionResult> ReceivePayment(PaymentDto p)
		{
			await _paymentService.PaymentProcess(p);
			var response = new PaymentResponse() { StatusMessage = "Ödeme Alındı" };
			return CreateActionResultInstance(ResponseDto<PaymentResponse>.Success(response, HttpStatusCode.OK.GetHashCode()));
		}
	}
}
