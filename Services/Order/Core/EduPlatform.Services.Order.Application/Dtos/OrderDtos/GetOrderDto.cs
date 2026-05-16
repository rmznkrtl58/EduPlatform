using EduPlatform.Services.Order.Application.Dtos.AddressDtos;
using EduPlatform.Services.Order.Application.Dtos.OrderItemDtos;
namespace EduPlatform.Services.Order.Application.Dtos.OrderDtos
{
	public class GetOrderDto
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get;  set; }
		public GetAddressDto Address { get;  set; }
		public string BuyerId { get;  set; }
		public List<GetOrderItemDto> OrderItems { get; set; }

	}
}
