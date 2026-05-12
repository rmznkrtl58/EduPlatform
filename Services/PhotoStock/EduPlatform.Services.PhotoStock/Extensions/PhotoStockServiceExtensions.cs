using EduPlatform.Services.PhotoStock.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace EduPlatform.Services.PhotoStock.Extensions
{
	public static class PhotoStockServiceExtensions
	{
		public static IServiceCollection AddPhotoStockRegistirations(this IServiceCollection services,IConfiguration configuration)
		{
			// Add services to the container.
			services.AddControllers(opt =>
			{
				//Global Authorize Filter Ekleme
				opt.Filters.Add(new AuthorizeFilter());
			});
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			services.AddScoped<IPhotoService, PhotoService>();
			//PhotoStock servicimi koruma altına alma
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
			{
				opt.Authority = configuration["IdentityServerURL"];
				opt.Audience = "resource_photo_Stock";
				opt.RequireHttpsMetadata = false;
			});
			return services;
		}
	}
}
