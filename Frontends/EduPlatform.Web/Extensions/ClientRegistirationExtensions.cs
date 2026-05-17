using EduPlatform.Web.Options;
using EduPlatform.Web.Services.IdentityServices;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EduPlatform.Web.Extensions
{
	public static class ClientRegistirationExtensions
	{

		private const string AuthSheme = CookieAuthenticationDefaults.AuthenticationScheme; 
		

		//Servic Registirations
		public static IServiceCollection AddCoreMvcRegistiration(this IServiceCollection services, IConfiguration configuration)
		{
			// Add services to the container.
			services.AddControllersWithViews();
			//ServiceApiSettings classımı DI'de IOptions ile geçtiğimde direk set edecek
			services.Configure<ServiceApiOptions>(configuration.GetSection(ServiceApiOptions.Key));
			services.Configure<ClientSettingOptions>(configuration.GetSection(ClientSettingOptions.Key));
			//IIdentity Service&Identity Service
			services.AddHttpContextAccessor();
            //scoped olarakta geçebilirdik ama classımın içinde httplicent kullandığım için böyle yazmam best practice
			services.AddHttpClient<IIdentityService, IdentityService>();
			//CookieBasedAuthentication
			services.AddAuthentication(AuthSheme).AddCookie(AuthSheme, opt =>
			{
				opt.LoginPath = "/Auth/SignIn";
				opt.ExpireTimeSpan = TimeSpan.FromDays(60);
				opt.SlidingExpiration = true;
				opt.Cookie.Name = "udemywebcookie";
			});

			return services;
		}


		

		//Middlewares
		public static IApplicationBuilder UseConfigurePipelineExt(this WebApplication app)
		{

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();

			return app;
		}
	}
}
