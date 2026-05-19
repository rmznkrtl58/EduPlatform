using EduPlatform.Web.Exceptions;
using EduPlatform.Web.Services.IdentityServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http.Headers;

namespace EduPlatform.Web.Handlers.ResourceOwnerCredentialHandler
{
	//Kullanıcı Adı Ve Şifre Girişli Senaryo
	public class ResourceOwnerTokenHandler:DelegatingHandler
	{
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IIdentityService _identityService;
		private readonly ILogger<ResourceOwnerTokenHandler> _logger;

		public ResourceOwnerTokenHandler(IHttpContextAccessor contextAccessor, IIdentityService identityService, ILogger<ResourceOwnerTokenHandler> logger)
		{
			_contextAccessor = contextAccessor;
			_identityService = identityService;
			_logger = logger;
		}
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{   //her bir istek başlatıldığında sendAsync metodum araya gircek ve çalışacak.


			//Cookieden tokenimi okudum
			var accesToken = await _contextAccessor.HttpContext!.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
			//spa frameworklerindeki mantıkda aynı gibi interceptorlarla araya girip okuduğumuz accesstokeni
			//isteğin headeriyla yolluyoruz.refreshle access doğru değilse logine yönlendiriyoruz.

			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accesToken);
			//attığım isteğin sonucunu alıyorum
			var response = await base.SendAsync(request, cancellationToken);
			//bu sonucu kontrol ediyorum eğerki benim accesstokenim geçersiz ise
			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				//refresh tokenle beraber yeni bir access token alıyorum.
				var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();
				if (tokenResponse is not null)
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer",tokenResponse.AccessToken);

					response=await base.SendAsync(request, cancellationToken);
				}
			}


			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				throw new UnAuthorizeException();
			}

			return response;
		}
	}
}
