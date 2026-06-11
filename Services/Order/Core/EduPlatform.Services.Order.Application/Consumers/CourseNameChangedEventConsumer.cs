using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Shared.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Services.Order.Application.Consumers
{
	public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
	{
		private readonly IOrderItemRepository _orderItemsRepo;
		private readonly IUnitOfWork _unitOfWork;
		public CourseNameChangedEventConsumer(IOrderItemRepository orderItemRepo, IUnitOfWork unitOfWork)
		{
			_orderItemsRepo = orderItemRepo;
			_unitOfWork = unitOfWork;
		}
		public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
		{
			//ilgili siparişin içerisindeki courseları ve özelliklerini aldım.
			var findOrderItems = await _orderItemsRepo.GetListByFilter(x => x.CourseId == context.Message.CourseId);
			foreach (var orderItem in findOrderItems.ToList())
			{
				orderItem.UpdateOrSave(context.Message.UpdatedName, orderItem.PictureUrl,orderItem.Price);
			}
			await _unitOfWork.CommitAsync();
		}
	}
}
