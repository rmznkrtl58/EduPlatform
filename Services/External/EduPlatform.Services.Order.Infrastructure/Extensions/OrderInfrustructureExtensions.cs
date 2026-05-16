using EduPlatform.Services.Order.Application.Contracts.Infrustructure.Repositories;
using EduPlatform.Services.Order.Domain.Options;
using EduPlatform.Services.Order.Infrastructure.Concrete.Repositories;
using EduPlatform.Services.Order.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace EduPlatform.Services.Order.Infrastructure.Extensions
{
	public static class OrderInfrustructureExtensions
	{
		public static IServiceCollection AddInfrustructure(this IServiceCollection services,IConfiguration configuration)
		{
		
			services.AddDbContext<AppDbContext>(opt =>
			{
				var sqlConnection = configuration.GetSection(SqlConnectionSettings.Key).Get<SqlConnectionSettings>();
				opt.UseSqlServer(sqlConnection.SqlServer, act => act.MigrationsAssembly(typeof(InfrastructureAssembly).Assembly.FullName));
			});
			//Life Cycles
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IOrderItemRepository, OrderItemRepository>();
			services.AddScoped<IAddressRepository, AddressRepository>();
			
			
			return services;
		}
	}
}
