using EduPlatform.Shared.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace EduPlatform.Services.Order.Application.Extensions
{
		public static class OrderApplicationExtensions
		{
			public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
			{
			    //MediatR
			    services.AddMediatR(typeof(ApplicationAssembly).Assembly);
			    services.AddScoped<ISharedIdentityService, SharedIdentityService>();
				return services;
			}
		}
	
}
