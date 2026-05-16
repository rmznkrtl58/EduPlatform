using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories
{
	public interface IOrderItemRepository : IGenericRepository<Order.Domain.OrderAggregate.OrderItem>
	{
	}
}
