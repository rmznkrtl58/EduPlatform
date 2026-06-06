using EduPlatform.Web.Models.FakePaymentViewModels;

namespace EduPlatform.Web.Services.FakePaymentServices
{
	public interface IPaymentService
	{
		Task<bool> ReceivePayment(PaymentInfoRequest p);
	}
}
