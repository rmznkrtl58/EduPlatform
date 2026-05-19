using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Models.AuthViewModels;
using EduPlatform.Web.Options;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace EduPlatform.Web.Services.IdentityServices
{
	public class IdentityService : IIdentityService
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly ClientSettingOptions _clientSettings;
		private readonly ServiceApiOptions _serviceApiSettings;
		public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor,IOptions<ClientSettingOptions>clientSettings,IOptions<ServiceApiOptions>serviceApiSettings)
		{
			_httpClient = httpClient;
			_contextAccessor = contextAccessor;
			_clientSettings = clientSettings.Value;
			_serviceApiSettings= serviceApiSettings.Value;
		}

		public async Task<TokenResponse> GetAccessTokenByRefreshToken()
		{
			//yeni bir refresh tokenimle hem cookieyi güncelliyorum hemde yeni bir access token alıyorum.

			//Identity Server endpintlere ulaşma
			var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
			{
				Address = _serviceApiSettings.IdentityBaseUri,
				//http üzerinden istek atacağımız için false set ettim.
				Policy = new DiscoveryPolicy() { RequireHttps = false }
			});
			if (disco.IsError) throw disco.Exception;

			var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
			var refreshTokenRequest = new RefreshTokenRequest()
			{
				ClientId = _clientSettings.WebClientForUser.ClientId,
				ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
				RefreshToken = refreshToken,
				Address = disco.TokenEndpoint
			};
			var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
			if(token.IsError)
			{
				new ErrorDto()
				{
					Errors = new List<string> { "Refresh Token oluşturmada hata oluştu" }
				};
			}
			
			var authenticationTokens=new List<AuthenticationToken>()
			{
				new AuthenticationToken()
				{
					Name=OpenIdConnectParameterNames.AccessToken,
					Value=token.AccessToken
				},
				new AuthenticationToken()
				{
					Name=OpenIdConnectParameterNames.RefreshToken,
					Value=token.RefreshToken
				},
				new AuthenticationToken()
				{
					Name=OpenIdConnectParameterNames.ExpiresIn,
					Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
				}
			};
			//Authentice olmuş kullanıcının bilgilerini cookiden aldık.
			var authenticationResult = await _contextAccessor.HttpContext.AuthenticateAsync();
			var properties = authenticationResult.Properties;
			properties.StoreTokens(authenticationTokens);

			await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

			return token;

		}

		public async Task RevokeRefreshToken()
		{
			//Kullanıcı çıkış yaptıktan sonra refresh token memoryden,db'den veya cookieden silmemiz gerek.

			//IdentityServerin belli başlı endpointlerine ulaşmak için,
			var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
			{
				Address = _serviceApiSettings.IdentityBaseUri,
				//http üzerinden istek atacağımız için false set ettim.
				Policy = new DiscoveryPolicy() { RequireHttps = false }
			});
			if (disco.IsError) throw disco.Exception;

			var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
			TokenRevocationRequest tokenRevocationRequest = new TokenRevocationRequest()
			{
				ClientId = _clientSettings.WebClientForUser.ClientId,
				ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
				Address = disco.RevocationEndpoint,
				Token = refreshToken,
				TokenTypeHint ="refresh_token"
			};

			await _httpClient.RevokeTokenAsync(tokenRevocationRequest);

		}

		public async Task<ResponseDto<bool>> SignIn(SignInRequestModel p)
		{
			//IdentityServerin belli başlı endpointlerine ulaşmak için,
			var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
			{
				Address = _serviceApiSettings.IdentityBaseUri,
				//http üzerinden istek atacağımız için false set ettim.
				Policy = new DiscoveryPolicy() { RequireHttps=false}
			});
			if (disco.IsError) throw disco.Exception;

			//Resource Owner Password parametrelerimi alacağım.
			var passwordTokenRequest = new PasswordTokenRequest()
			{
				ClientId = _clientSettings.WebClientForUser.ClientId,
				ClientSecret=_clientSettings.WebClientForUser.ClientSecret,
				UserName=p.Email,
				Password=p.Password,
				Address=disco.TokenEndpoint
			};

			var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
			if (token.IsError)
			{
				var responseContent=await token.HttpResponse.Content.ReadAsStringAsync();
				var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent,
					new JsonSerializerOptions() 
					{ 
						//Küçük harf büyük harf şartı koşma
						PropertyNameCaseInsensitive = true 
					});

				return ResponseDto<bool>.Fail(errorDto.Errors, HttpStatusCode.BadRequest.GetHashCode());
			}

			var userInfoRequest = new UserInfoRequest()
			{
				Token = token.AccessToken,
				Address = disco.UserInfoEndpoint
			};
			var userInfo = await _httpClient.GetUserInfoAsync(userInfoRequest);
			if (userInfo.IsError) throw userInfo.Exception;

			var claimsIdentity = new ClaimsIdentity
				(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme,JwtRegisteredClaimNames.Name,"role");

			//Cookie iskeletini temelini oluşturucam
			ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

			var authenticationProperties = new AuthenticationProperties();
			//tokenlerimi cookide saklamak için gerekli yapılandırmalarımı yapıyorum
			authenticationProperties.StoreTokens(new List<AuthenticationToken>()
			{
				new AuthenticationToken()
				{
					Name=OpenIdConnectParameterNames.AccessToken,
					Value=token.AccessToken
				},
				new AuthenticationToken()
				{
					Name=OpenIdConnectParameterNames.RefreshToken,
					Value=token.RefreshToken
				},
				new AuthenticationToken()
				{
					Name=OpenIdConnectParameterNames.ExpiresIn,
					Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
				}
			});
			authenticationProperties.IsPersistent = p.IsRemember;

			await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);
			return ResponseDto<bool>.Success(true, HttpStatusCode.OK.GetHashCode());
		}
		public async  Task LogOut()
		{
			await _contextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			await RevokeRefreshToken();
		}
	}
}
