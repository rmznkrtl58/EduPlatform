
using IdentityModel.Client;

namespace EduPlatform.Gateway.DelegateHandler
{
	public class TokenExchangeDelegateHandler:DelegatingHandler
	{
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;
		private string _accessToken;
		public TokenExchangeDelegateHandler(IConfiguration configuration, HttpClient httpClient)
		{
			_configuration = configuration;
			_httpClient = httpClient;
		}
		//Bana istek yaptığımızda gelen tokeni ver bende IdentityServera iletim
		//yeni token alayım
		public async Task<string> GetToken(string requestToken)
		{
			if (!string.IsNullOrEmpty(_accessToken))
			{
				return _accessToken;
			}

			var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
			{
				Address = _configuration["IdentityServerURL"],
				Policy = new DiscoveryPolicy { RequireHttps = false }
			});

			if (disco.IsError)
			{
				throw disco.Exception;
			}

			//Değiştirilecek tokenim için gerekli bilgileri burda veriyorum.
			TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest()
			{
				Address = disco.TokenEndpoint,
				ClientId = _configuration["ClientId"],
				ClientSecret = _configuration["ClientSecret"],
				GrantType = _configuration["TokenGrantType"],
				//Değiştirilmesi istenen tokenim
				SubjectToken = requestToken,
				//Değiştirilmesi istenen tokenimin tipi
				SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
				//Değiştirilip yerine geçecek olan tokenim hangi izinlere sahip
				//veya hangi servislere istek atabilir.
				Scope = "openid discount_fullpermission payment_fullpermission"
			};

			//eski tokenin yerine geçecek tokenim.
			var tokenResponse =await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
			if (tokenResponse.IsError) throw tokenResponse.Exception;

			_accessToken = tokenResponse.AccessToken;
			return _accessToken;
		}
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			//eski tokeni alıyorum.
			var requestToken = request.Headers.Authorization.Parameter;
			
			//eski tokeni iade edip yenisini alıyorum.çünkü eski tokenimde Sadece order ile baskete istek
			//yapabilir ama discount ile fakepaymente istek yapamaz.
			//yeni alacağım tokende ise eski tokendeki izinler yerine discount ve fakepaymenta istek atacağım
			//izinlerin bulunduğu token olacak.
			var newToken = await GetToken(requestToken);
			
			//IdentityModel paketi httpClient üzerinden extension metodlar getiriyor.(yardımcı metodlar)
			request.SetBearerToken(newToken);
			
			return await base.SendAsync(request, cancellationToken);
		}
	}
}
