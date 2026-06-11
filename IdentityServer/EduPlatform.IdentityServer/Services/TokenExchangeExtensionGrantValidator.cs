using IdentityServer4.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduPlatform.IdentityServer.Services
{
	public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
	{
		public string GrantType =>"urn:ietf:params:oauth:grant-type:token-exchange";
		//Gelen Tokeni doğrulayalım.
		private readonly ITokenValidator _tokenValidator;

		public TokenExchangeExtensionGrantValidator(ITokenValidator tokenValidator)
		{
			_tokenValidator = tokenValidator;
		}

		public async Task ValidateAsync(ExtensionGrantValidationContext context)
		{
			//tokene yapacağım isteği aldım
			var requestRaw =context.Request.Raw.ToString();
			
			//istek yapıldı ama token döndümü?
			var token = context.Request.Raw.Get("subject_token");
			if (string.IsNullOrEmpty(token))
			{
				context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidRequest, "token missing");
				return;
			}

			//elimizde token varsa bunu doğrulayalım
			var tokenValidateResult = await _tokenValidator.ValidateAccessTokenAsync(token);
			if (tokenValidateResult.IsError)
			{
				context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant, "token invalid");
				return;
			}

			//Resource owner password yani kullanıcıya ait bir token olmalı.
			var subjectClaim = tokenValidateResult.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
			if(subjectClaim is null)
			{
				//Resource Owner Pasword olmayan.
				context.Result = new GrantValidationResult(IdentityServer4.Models.TokenRequestErrors.InvalidGrant
					,"Token bir kullanıcıya ait olmalıdır.");
				return;
			}

			//kullanıcının Id'si,Access Token,Ve authentice olmuş olan kullanıcının claimleri verdim.
			context.Result = new GrantValidationResult(subjectClaim.Value, "access_token", tokenValidateResult.Claims);
			return;
		}
	}
}
