

namespace EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories
{
	public interface IOrderRepository : IGenericRepository<Order.Domain.OrderAggregate.Order>
	{
		Task<List<Order.Domain.OrderAggregate.Order>> GetListByUserIdWithOrderItems(string userId);
	}
}
