using EduPlatform.Services.Catalog.Services;
using EduPlatform.Services.Catalog.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;

namespace EduPlatform.Services.Catalog.Extensions
{
	public static class CatalogServiceRegistirationExtensions
	{
		public static IServiceCollection AddCatalogRegistirations(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddControllers(opt =>
			{
				//global authorize filter
				opt.Filters.Add(new AuthorizeFilter());
			});
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			//Mapper
			services.AddAutoMapper(typeof(CatalogAssembly).Assembly);
			//MongoDb
			services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.Key));
			services.AddSingleton<IDatabaseSettings>(sp =>
			{
				return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
			});
			//LifeTimeCycles
			services.AddScoped<ICategoryService,CategoryService >();
			services.AddScoped<ICourseService,CourseService >();
			//JwtBearer Configuration mikroservicimi koruma altına alma.
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
			{
				opt.Authority = configuration["IdentityServerURL"];
				opt.Audience = "resource_catalog";
				opt.RequireHttpsMetadata = false;
			});

			return services;
		}
	}
}
