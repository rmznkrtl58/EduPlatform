namespace EduPlatform.Services.FakePayment.Models
{
	public class OrderDto
	{
		public OrderDto()
		{
			OrderItems = new List<OrderItemDto>();
		}
		public string BuyerId { get; set; }
		public List<OrderItemDto> OrderItems { get; set; }
		public AddressDto Address { get; set; }
	}
	public class AddressDto
	{
		public string Province { get; set; }
		public string District { get; set; }
		public string Street { get; set; }
		public string ZipKode { get; set; }
		public string AddressDetail { get; set; }
	}
	public class OrderItemDto
	{
		public string CourseId { get; set; }
		public string CourseName { get; set; }
		public string PictureUrl { get; set; }
		public decimal Price { get; set; }
	}
}
