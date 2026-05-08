using EduPlatform.Services.Catalog.Services;
using EduPlatform.Services.Catalog.Settings;
using Microsoft.Extensions.Options;

namespace EduPlatform.Services.Catalog.Extensions
{
	public static class CatalogServiceRegistirationExtensions
	{
		public static IServiceCollection AddCatalogRegistirations(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddControllers();
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
			return services;
		}
	}
}
