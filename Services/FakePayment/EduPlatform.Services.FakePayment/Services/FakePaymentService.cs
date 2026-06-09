using EduPlatform.Services.FakePayment.Models;
using EduPlatform.Shared.Messages;
using MassTransit;

namespace EduPlatform.Services.FakePayment.Services
{
	public class FakePaymentService
	{
		//Eventler için geçerlidir :D şuan kullanmaya gerek yok örnek olsun...
		//private readonly IPublishEndpointProvider _publishEndpoint;
		private readonly ISendEndpointProvider _sendEndpointProvider;
		public FakePaymentService(ISendEndpointProvider sendEndpointProvider)
		{
			_sendEndpointProvider = sendEndpointProvider;
		}
		//Ordera haberi burdan ileteceğiz rabbitMq kullanarak
		//mesaj tipi olarakta command tipi kullanıyoruz.
		
		public async Task PaymentProcess(PaymentDto p)
		{
			//bir command göndereceğim için send kullandım.
			var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));
			var createOrderMessageCommand = new CreateOrderMessageCommand()
			{
				BuyerId = p.Order.BuyerId,
				Address = new GetAddress()
				{
					AddressDetail = p.Order.Address.AddressDetail,
					District = p.Order.Address.District,
					Province = p.Order.Address.Province,
					Street = p.Order.Address.Street,
					ZipKode = p.Order.Address.ZipKode
				}
			};
			p.Order.OrderItems.ForEach(orderItem =>
			{
				var getOrderItem = new GetOrderItem()
				{
					CourseId = orderItem.CourseId,
					CourseName = orderItem.CourseName,
					PictureUrl = orderItem.PictureUrl,
					Price = orderItem.Price
				};
				createOrderMessageCommand.OrderItems.Add(getOrderItem);
			});

			await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
		}
	}
}
