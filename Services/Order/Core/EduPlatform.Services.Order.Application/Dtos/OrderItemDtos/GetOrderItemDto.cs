namespace EduPlatform.Services.Order.Application.Dtos.OrderItemDtos
{
	public class GetOrderItemDto
	{
		public string CourseId { get;  set; }
		public string CourseName { get;  set; }
		public string PictureUrl { get;  set; }
		public decimal Price { get;  set; }
	}
}
