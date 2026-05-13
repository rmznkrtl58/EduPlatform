using Microsoft.AspNetCore.Http;

namespace EduPlatform.Shared.Services
{
	public sealed class SharedIdentityService : ISharedIdentityService
	{
		//burdaki kullandığım http context accessor uygulamanın kalbidir buradan hem requeste hemde response tarafına
		//ulaşabilirim
		private IHttpContextAccessor _contextAccessor;
		public SharedIdentityService(IHttpContextAccessor contextAccessor)
		{
			_contextAccessor = contextAccessor;
		}
		public string GetUserId => _contextAccessor.HttpContext.User.FindFirst("sub").Value;
	}
}
