
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace EduPlatform.Services.Order.Extensions
{
	public static class OrderApiExtensions
	{
		public static IServiceCollection AddOrderApi(this IServiceCollection services, IConfiguration configuration)
		{
			//JwtBearer Configuration mikroservicimi koruma altına alma.
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
			{
				opt.Authority = configuration["IdentityServerURL"];
				opt.Audience = "resource_order";
				opt.RequireHttpsMetadata = false;
			});
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
			//Default Configurations
			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddHttpContextAccessor();
			return services;
		}
	}
}
