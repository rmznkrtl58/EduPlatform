using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace EduPlatform.Gateway.Extensions
{
	public static class OcelotGatewayRegistirations
	{
		public static IServiceCollection AddOcelotRegistiration(this IServiceCollection services,IConfiguration configuration,WebApplicationBuilder builder)
		{
			//benim ocelat.development.json dosyamdaki AuthenticationProviderKey değeri burdaki sheme'mı kapsıyor
			builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", options =>
			{
				options.Authority = builder.Configuration["IdentityServerURL"];
				options.Audience = "resource_gateway";
				options.RequireHttpsMetadata = false;
			});
			//builder.Services.AddHttpClient<TokenExhangeDelegateHandler>();
		    builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json");
			services.AddOcelot();
			//builder.Services.AddOcelot().AddDelegatingHandler<TokenExhangeDelegateHandler>();
			return services;
		}
		public async static Task<IApplicationBuilder> UseConfigurePipelineExt(this WebApplication app)
		{
			app.UseAuthorization();
			app.UseDeveloperExceptionPage();
			app.MapControllers();

			await app.UseOcelot();

			app.Run();

			return app;
		}
	}
}
