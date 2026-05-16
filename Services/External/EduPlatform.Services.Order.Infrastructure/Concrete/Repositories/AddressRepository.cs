using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Domain.OrderAggregate;
using EduPlatform.Services.Order.Infrastructure.Data;

namespace EduPlatform.Services.Order.Infrastructure.Concrete.Repositories
{
	internal class AddressRepository : GenericRepository<Address>, IAddressRepository
	{
		public AddressRepository(AppDbContext context) : base(context)
		{
		}
	}
}
