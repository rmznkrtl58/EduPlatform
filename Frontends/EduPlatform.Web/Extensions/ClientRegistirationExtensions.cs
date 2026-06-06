using EduPlatform.Shared.Services;
using EduPlatform.Web.Handlers.ClientCredentialHandler;
using EduPlatform.Web.Handlers.ResourceOwnerCredentialHandler;
using EduPlatform.Web.Helpers;
using EduPlatform.Web.Options;
using EduPlatform.Web.Services.BasketServices;
using EduPlatform.Web.Services.CatalogServices.CategoryServices;
using EduPlatform.Web.Services.CatalogServices.CourseServices;
using EduPlatform.Web.Services.ClientCredentialServices;
using EduPlatform.Web.Services.DiscountServices;
using EduPlatform.Web.Services.FakePaymentServices;
using EduPlatform.Web.Services.IdentityServices;
using EduPlatform.Web.Services.OrderServices;
using EduPlatform.Web.Services.UserServices;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
namespace EduPlatform.Web.Extensions
{
	public static class ClientRegistirationExtensions
	{
		private const string AuthSheme = CookieAuthenticationDefaults.AuthenticationScheme; 
		//Servic Registirations
		//this keywordu kim için extension yazıyorsun demek.
		public static IServiceCollection AddCoreMvcRegistiration(this IServiceCollection services, IConfiguration configuration)
		{
			//AddHttpClient çok tekrarlı kod vardı metod içine çektim
			AddHttpClients(services,configuration);
			// Add services to the container.
			services.AddControllersWithViews().AddFluentValidation(fv =>
			{   //FluentApi Registiration
				fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			});
			//ServiceApiSettings classımı DI'de IOptions ile geçtiğimde direk set edecek
		    services.Configure<ServiceApiOptions>(configuration.GetSection(ServiceApiOptions.Key));
			services.Configure<ClientSettingOptions>(configuration.GetSection(ClientSettingOptions.Key));
			//IIdentity Service&Identity Service
			services.AddHttpContextAccessor();
			services.AddScoped<ResourceOwnerTokenHandler>();
			services.AddScoped<ClientCredentialTokenHandler>();
			services.AddScoped<ISharedIdentityService,SharedIdentityService>();
			//IClientAccessTokenCache işlemlerinin çalışması için.
			services.AddAccessTokenManagement();
			//CookieBasedAuthentication
			services.AddAuthentication(AuthSheme).AddCookie(AuthSheme, opt =>
			{
				opt.LoginPath = "/Auth/SignIn";
				opt.ExpireTimeSpan = TimeSpan.FromDays(60);
				opt.SlidingExpiration = true;
				opt.Cookie.Name = "udemywebcookie";
			});
			//PhotoUrl Almak için yapılandırma
			services.AddSingleton<PhotoHelper>();
			return services;
		}
		//HttpClients
		public static void AddHttpClients(IServiceCollection services,IConfiguration configuration)
		{
			//scoped olarakta geçebilirdik ama classımın içinde httplicent kullandığım için böyle yazmam best practice
			services.AddHttpClient<IIdentityService, IdentityService>();
			var serviceApiSettings=configuration.GetSection(ServiceApiOptions.Key).Get<ServiceApiOptions>();
			services.AddHttpClient<IUserService, UserService>(opt =>
			{
				opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
			}).AddHttpMessageHandler<ResourceOwnerTokenHandler>();
			services.AddHttpClient<IBasketService, BasketService>(opt =>
			{
				opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
			}).AddHttpMessageHandler<ResourceOwnerTokenHandler>();
			services.AddHttpClient<IPaymentService, PaymentService>(opt =>
			{
				opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Payment.Path}");
			}).AddHttpMessageHandler<ResourceOwnerTokenHandler>();
			services.AddHttpClient<IOrderService, OrderService>(opt =>
			{
				opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Order.Path}");
			}).AddHttpMessageHandler<ResourceOwnerTokenHandler>();
			services.AddHttpClient<IDiscountService, DiscountService>(opt =>
			{
				opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Discount.Path}");
			}).AddHttpMessageHandler<ResourceOwnerTokenHandler>();
			services.AddHttpClient<ICourseService, CourseService>(opt =>
			{
				opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
			}).AddHttpMessageHandler<ClientCredentialTokenHandler>();
			services.AddHttpClient<ICategoryService, CategoryService>(opt =>
			{
				opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
			}).AddHttpMessageHandler<ClientCredentialTokenHandler>();
			services.AddHttpClient<IClientCredentialTokenService,ClientCredentialTokenService>();
		}
		//Middlewares
		public static IApplicationBuilder UseConfigurePipelineExt(this WebApplication app)
		{

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				//exception fırlayacağı zaman bu sayfaya yönlenecek aslında bunun olayı production
				//ortamda kullanıcıların gözüne batmadan direk sayfaya yönlenmesi custom yazdığın
				//exceptionlara göre ayar çekebilirsin zaten bir unauthorize için yazmıştık
				//bunun burda durması faydalı olacaktır ama denemek istenirse exception testi için 
				//bu süslü parantezin dışınada çıkıp test edilebilir.
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
