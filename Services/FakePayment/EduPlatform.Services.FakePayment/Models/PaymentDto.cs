namespace EduPlatform.Services.FakePayment.Models
{
	public class PaymentDto
	{
		public string CardName { get; set; }
		public string CardNumber { get; set; }
		public DateTime Expiration { get; set; }
		public string CVV { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
