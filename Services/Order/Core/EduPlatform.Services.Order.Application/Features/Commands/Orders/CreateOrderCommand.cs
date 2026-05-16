using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Application.Dtos.AddressDtos;
using EduPlatform.Services.Order.Application.Dtos.OrderDtos;
using EduPlatform.Services.Order.Application.Dtos.OrderItemDtos;
using EduPlatform.Services.Order.Domain.OrderAggregate;
using EduPlatform.Shared.Dtos;
using Mapster;
using MediatR;
using System.Net;
namespace EduPlatform.Services.Order.Application.Features.Commands.Orders
{
	public class CreateOrderCommand:IRequest<ResponseDto<CreatedOrderDto>>
	{
		public string BuyerId { get; set; }
		public List<GetOrderItemDto> OrderItems { get; set; }
		public GetAddressDto Address { get; set; }
	}
	public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseDto<CreatedOrderDto>>
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IUnitOfWork _unitOfWork;
		public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
		{
			_orderRepository = orderRepository;
			_unitOfWork = unitOfWork;
		}
		public async Task<ResponseDto<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
		{
			//domain driven desing yaklaşımı normal eskisinden farklı farkettiysek önemli olan mantık şu
			//orderimi oluştururken diğer nesneleride oluşturuyorum ve order entitiymi diğer ilişkili oldukları
			//yapılarla bir bütün halinde işlemlerime devam ediyorum
			
			var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipKode, request.Address.AddressDetail);
			var newOrder = new Order.Domain.OrderAggregate.Order(newAddress, request.BuyerId);

			//parametremden gelen sipariş itemlerim üzerinden ilerleyeceğim.
			request.OrderItems.ForEach(item =>
			{
				//burdada benim order entityim içerisindeki yazmış olduğum siparişe item ekle metodumu çağırıyorum.
				newOrder.AddOrderItem(item.CourseId, item.CourseName, item.Price, item.PictureUrl);
			});
			await _orderRepository.CreateAsync(newOrder);
			await _unitOfWork.CommitAsync();
			var response = new CreatedOrderDto() { OrderId = newOrder.Id };
			return ResponseDto<CreatedOrderDto>.Success(response, HttpStatusCode.Created.GetHashCode());
		}
	}
}
