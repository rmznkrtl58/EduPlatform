using EduPlatform.Web.Models.AuthViewModels;

namespace EduPlatform.Web.Services.UserServices
{
	public interface IUserService
	{
		Task<UserViewModel> GetUser();
	}
}
