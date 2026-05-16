using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Services.Order.Infrastructure.Concrete.Repositories
{
	internal class OrderRepository : GenericRepository<Order.Domain.OrderAggregate.Order>, IOrderRepository
	{
		public OrderRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<List<Domain.OrderAggregate.Order>> GetListByUserIdWithOrderItems(string userId)
		{
			return await _dbSet.Where(x => x.BuyerId == userId).Include(x => x.OrderItems).ToListAsync();		
		}
	}
}
