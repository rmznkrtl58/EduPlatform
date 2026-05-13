using EduPlatform.Services.Basket.Services;
using EduPlatform.Services.Basket.Settings;
using EduPlatform.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace EduPlatform.Services.Basket.Extensions
{
	public static class BaskerServiceExtensions
	{
		public static IServiceCollection AddBasketRegistirations(this IServiceCollection services,IConfiguration configuration)
		{
			//mikroservisi koruma
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
			{
					opt.Authority = configuration["IdentityServerURL"];
					opt.Audience = "resource_basket";
					opt.RequireHttpsMetadata = false;
			});
			//Jwt içerisindeki sub keyine ait kullanıcı Id valuemi maplame işlemini düzeltmek için
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
			//Global Authorize işlemi 
			var authorizePolicy = new
				AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser().Build();//mutlaka authentice olmuş kullanıcı gerekli.
			services.AddControllers(opt =>
			{
				//Global Authorize filter
				opt.Filters.Add(new AuthorizeFilter(authorizePolicy));
			});
			//swagger
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			//Redis dbye bağlanırken redisSettings classımı gördüğün yerde atamayı burda yapacam.
			services.Configure<RedisSettings>(configuration.GetSection(RedisSettings.Key));
			//RedisConfiguration
			services.AddSingleton<RedisService>(sp =>
			{
				var redisSettings=sp.GetRequiredService<IOptions<RedisSettings>>().Value;
				var redis = new RedisService(redisSettings.Host, redisSettings.Port);
				redis.Connect();
				return redis;
			});
			services.AddScoped<IBasketService, BasketService>();
			//Claim nesneme ulaşmak ve userId'imi almak ve users lara ulaşmak için bunu kullanıyom
			services.AddHttpContextAccessor();
			services.AddScoped<ISharedIdentityService,SharedIdentityService>();
			return services;
		}
	}
}
