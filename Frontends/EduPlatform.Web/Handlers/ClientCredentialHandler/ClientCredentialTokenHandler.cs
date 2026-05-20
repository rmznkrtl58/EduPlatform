
using EduPlatform.Web.Exceptions;
using EduPlatform.Web.Services.ClientCredentialServices;
using System.Net;

namespace EduPlatform.Web.Handlers.ClientCredentialHandler
{
	public class ClientCredentialTokenHandler:DelegatingHandler
	{
		//Catalog ve Photo stock için geçerli kullanıcı üyeliği içermeyen client credential bilgilerini 
		//yollayıp token doğrulaması yapacağım yer.

		private readonly IClientCredentialTokenService _clientCredentialTokenService;
		public ClientCredentialTokenHandler(IClientCredentialTokenService clientCredentialTokenService)
		{
			_clientCredentialTokenService = clientCredentialTokenService;
		}
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{   //Memorydeki tokenimi requestin headerine koyacağım.

			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await _clientCredentialTokenService.GetToken());

			var response=await base.SendAsync(request, cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized) throw new UnAuthorizeException();

			return response;
		}
	}
}
