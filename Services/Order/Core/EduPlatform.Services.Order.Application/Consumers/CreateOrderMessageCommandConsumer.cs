using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Shared.Messages;
using MassTransit;

namespace EduPlatform.Services.Order.Application.Consumers
{
	//IConsumer<CreateOrderMessageCommand> burdaki CreateOrderMessageCommand
	//Shared librarydeki ortak namespace içerisinde olduğu için kuyruktan direk olarak dinleyecek.
	internal class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
	{
		private readonly IOrderRepository _orderRepo;
		private readonly IUnitOfWork _unitOfWork;
		public CreateOrderMessageCommandConsumer(IOrderRepository orderRepo, IUnitOfWork unitOfWork)
		{
			_orderRepo = orderRepo;
			_unitOfWork = unitOfWork;
		}
		public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
		{
			//Siparişimizin adres kısmını set ettik
			var newAddress = 
				new Order.Domain.OrderAggregate.Address(context.Message.Address.Province, context.Message.Address.District, context.Message.Address.Street, context.Message.Address.ZipKode, context.Message.Address.AddressDetail);
			
			//sipariş classımızı oluşturup gerekli yerleri set ettik.
			var order = new Order.Domain.OrderAggregate.Order(newAddress, context.Message.BuyerId);
			context.Message.OrderItems.ForEach(orderItem =>
			{
				order.AddOrderItem(orderItem.CourseId, orderItem.CourseName, orderItem.Price, orderItem.PictureUrl);
			});
			
			await _orderRepo.CreateAsync(order);
			await _unitOfWork.CommitAsync();
		}
	}
}
