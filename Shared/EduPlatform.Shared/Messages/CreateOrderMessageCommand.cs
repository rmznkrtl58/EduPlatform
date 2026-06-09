using System.Collections.Generic;

namespace EduPlatform.Shared.Messages
{
	//bir siparişin oluşması için gerekli olan propları yazdım.
	//çünkü ordera ait datayı ilgili order service iletmem lazım.
	public class CreateOrderMessageCommand
	{
		public CreateOrderMessageCommand()
		{
			OrderItems = new List<GetOrderItem>();
		}
		public string BuyerId { get; set; }
		public List<GetOrderItem> OrderItems { get; set; }
		public GetAddress Address { get; set; }
	}

	public class GetOrderItem
	{
		public string CourseId { get; set; }
		public string CourseName { get; set; }
		public string PictureUrl { get; set; }
		public decimal Price { get; set; }
	}

	public class GetAddress
	{
		public string Province { get; set; }
		public string District { get; set; }
		public string Street { get; set; }
		public string ZipKode { get; set; }
		public string AddressDetail { get; set; }
	}
}
