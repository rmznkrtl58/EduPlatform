
using EduPlatform.Web.Options;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace EduPlatform.Web.Services.ClientCredentialServices
{
	public class ClientCredentialTokenService : IClientCredentialTokenService
	{
		private readonly ServiceApiOptions _serviceApiSettings;
		private readonly ClientSettingOptions _clientSettings;
		private readonly IClientAccessTokenCache _clientAccessTokenCache;
		private readonly HttpClient _httpClient;

		public ClientCredentialTokenService(IOptions<ServiceApiOptions> serviceApiSettings, IOptions<ClientSettingOptions> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
		{
			_serviceApiSettings = serviceApiSettings.Value;
			_clientSettings = clientSettings.Value;
			_clientAccessTokenCache = clientAccessTokenCache;
			_httpClient = httpClient;
		}

		public async Task<string> GetToken()
		{
			//memoryde access token var mı?
			var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken");
			if (currentToken is not null) return currentToken.AccessToken;
			//identityserverimin genel endpointlerinin bulunduğu metod
			var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
			{
				Address = _serviceApiSettings.IdentityBaseUri,
				Policy = new DiscoveryPolicy() { RequireHttps=false}
			});
			if (disco.IsError) throw disco.Exception;
			//Kullanıcı authentication zorunlu olmayan sistemde token üretmesini istiyorum.
			var clientCredentialTokenRequest = new ClientCredentialsTokenRequest()
			{
				ClientId = _clientSettings.WebClient.ClientId,
				ClientSecret = _clientSettings.WebClient.ClientSecret,
				Address = disco.TokenEndpoint
			};

			var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
			if (newToken.IsError) throw newToken.Exception;

			//herşey yolunda gitti ve tokenim oluştuysa memorye kaydetme zamanı!
			await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn);
			
			return newToken.AccessToken;
		}
	}
}
