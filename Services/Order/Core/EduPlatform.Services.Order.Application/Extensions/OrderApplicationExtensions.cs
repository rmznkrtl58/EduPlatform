using EduPlatform.Services.Order.Application.Consumers;
using EduPlatform.Shared.Services;
using MassTransit;
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
			//fakepayment üzerinden kuyruğa gelen ödeme işlemini yolladık
			//rabbitMq ile order servicede dinleyip gerekli mesaj üzerinden
			//sipariş oluşturma talebini ilettik şimdi configurasyon işlemi
			//RabbitMq 
			services.AddMassTransit(x =>
			{

				//mesajı dinleyeni classı tanımla
				x.AddConsumer<CreateOrderMessageCommandConsumer>();

				x.UsingRabbitMq((context, cfg) =>
				{
					//Default port:5672
					cfg.Host(configuration["RabbitMQUrl"], "/", host =>
					{
						//Default olarak guest'tir kullanıcı adı ve şifre 
						//kendi belirlediğinide kullanabilirsin.
						host.Username("guest");
						host.Password("guest");
					});


					//fake payment serviste belirlemiş olduğumuz kuyruğun ismiyle çağırıyoruz.
					cfg.ReceiveEndpoint("create-order-service", configureEndpoint =>
					{
						configureEndpoint.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
					});

				});
			});
			services.AddMassTransitHostedService();
			
			return services;
			}
		}
	
}
