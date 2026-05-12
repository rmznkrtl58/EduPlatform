using EduPlatform.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EduPlatform.IdentityServer.Services
{
	public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
	{
		//resource owner credentials grant tipinde bir istek yaptığımızda buradaki yazdığımı
		//işlemler devreye girecek Identity servere cevap verecek.

		private readonly UserManager<ApplicationUser> _userManager;
		public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		//ilgili tokeni vermesi için username ve password kontrolü yapacağız.
		public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
		{
			//username yazdığına bakma ben orda email döneceğim
			var findUser = await _userManager.FindByEmailAsync(context.UserName);
			if(findUser is null)//eğerki gönderdiğim email veya kullanıcı adı db'de yoksa
			{
				var errors = new Dictionary<string, object>();
				errors.Add("errors", new List<string> { "Eposta Veya Şifre Hatalıdır." });
				context.Result.CustomResponse= errors;
				
				return;
			}

			var checkPassword = await _userManager.CheckPasswordAsync(findUser, context.Password);
			if (!checkPassword)
			{
				var errors = new Dictionary<string, object>();
				errors.Add("errors", new List<string> { "Eposta Veya Şifre Hatalıdır." });
				context.Result.CustomResponse = errors;

				return;
			}
			//kullanıcı adı veya email ve şifre doğruysa bu adıma geçecek buradada IdentityServerim
			//kullanıcı adı ve şifrenin doğruluğunu tespit edip tokenimi verecek.
			context.Result = new GrantValidationResult
				(findUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
		}
	}
}

