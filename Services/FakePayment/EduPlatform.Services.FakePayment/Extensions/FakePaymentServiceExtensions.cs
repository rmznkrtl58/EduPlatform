using EduPlatform.Services.FakePayment.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace EduPlatform.Services.FakePayment.Extensions
{
	public static class FakePaymentServiceExtensions
	{
		public static IServiceCollection AddFakePayment(this IServiceCollection services,IConfiguration configuration)
		{
			//Default
			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			//JwtBearer Configuration mikroservicimi koruma altına alma.
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
			{
				opt.Authority =configuration["IdentityServerURL"];
				opt.Audience = "resource_fake_payment";
				opt.RequireHttpsMetadata = false;
			});
			services.AddScoped<FakePaymentService>();
			//Jwt içerisindeki sub keyine ait kullanıcı Id valuemi maplame işlemini düzeltmek için
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
			//Default Controllers
			var authorizePolicy = new
				AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser().Build();//mutlaka authentice olmuş kullanıcı gerekli.
			services.AddControllers(opt =>
			{
				//global authorize filter
				opt.Filters.Add(new AuthorizeFilter(authorizePolicy));
			});
			//RabbitMq 
			services.AddMassTransit(x =>
			{
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
				});
			});
			services.AddMassTransitHostedService();

			return services;
		}
	}
}
