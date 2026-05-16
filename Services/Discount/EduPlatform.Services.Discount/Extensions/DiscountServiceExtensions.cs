using EduPlatform.Services.Discount.Services;
using EduPlatform.Services.Discount.Settings;
using EduPlatform.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace EduPlatform.Services.Discount.Extensions
{
	public static class DiscountServiceExtensions
	{
		public static IServiceCollection AddDiscountRegistirations(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.Configure<PostgreSqlSettings>(configuration.GetSection(PostgreSqlSettings.Key));
			services.AddScoped<IDiscountService, DiscountService>();
			//eğer izinler konusunda sadece discount için read olanları istediğim zaman aşağıdaki policyi yazıyorum
			//var authorizePolicyExample = new AuthorizationPolicyBuilder().RequireClaim("scope", "discount_read_permission");
			//Global Authorize işlemi 
			var authorizePolicy = new
				AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser().Build();//mutlaka authentice olmuş kullanıcı gerekli.
			services.AddControllers(opt =>
			{
				//Global Authorize filter
				opt.Filters.Add(new AuthorizeFilter(authorizePolicy));
			});
			//mikroservisi koruma
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
			{
				opt.Authority = configuration["IdentityServerURL"];
				opt.Audience = "resource_discount";
				opt.RequireHttpsMetadata = false;
			});
			//Jwt içerisindeki sub keyine ait kullanıcı Id valuemi maplame işlemini düzeltmek için
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
			//SharedLibraryde kullandığım usera ait Id değerine ulaşacağım propum için tanımlıyorum
			services.AddHttpContextAccessor();
			services.AddScoped<ISharedIdentityService, SharedIdentityService>();
			return services;
		}
	}
}
