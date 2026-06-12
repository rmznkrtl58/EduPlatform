
using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Infrastructure.Data;

namespace EduPlatform.Services.Order.Infrastructure.Concrete.Repositories
{
	internal class OrderItemRepository : GenericRepository<Order.Domain.OrderAggregate.OrderItem>, IOrderItemRepository
	{
		public OrderItemRepository(AppDbContext context) : base(context)
		{
		}
	}
}
