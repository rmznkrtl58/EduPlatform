using EduPlatform.Shared.Dtos;
using EduPlatform.Web.Models.AuthViewModels;
using IdentityModel.Client;

namespace EduPlatform.Web.Services.IdentityServices
{
	public interface IIdentityService
	{
		Task<ResponseDto<bool>> SignIn(SignInRequestModel p);
		Task<TokenResponse> GetAccessTokenByRefreshToken();
		//Logout olduğunda kullanıcının dbde tutulan refresh tokeni uçuralım
		Task RevokeRefreshToken();
		Task LogOut();
	}
}
