using EduPlatform.Services.FakePayment.Models;
using EduPlatform.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace EduPlatform.Services.FakePayment.Controllers
{
	public class FakePaymentsController : CustomBaseController
	{   
		
		[HttpPost]
		public IActionResult ReceivePayment()
		{
			var response = new PaymentResponse() { StatusMessage = "Ödeme Alındı" };
			return CreateActionResultInstance(ResponseDto<PaymentResponse>.Success(response, HttpStatusCode.OK.GetHashCode()));
		}
	}
}
