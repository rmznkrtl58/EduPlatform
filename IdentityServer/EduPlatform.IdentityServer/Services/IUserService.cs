using EduPlatform.IdentityServer.Dtos;
using EduPlatform.IdentityServer.Models;
using EduPlatform.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EduPlatform.IdentityServer.Services
{
	public interface IUserService
	{
		Task<ResponseDto<ResponseMessageDto>> CreateUserAsync(CreateUserDto p);
		Task<ResponseDto<GetUserInfoDto>> GetUserInformationAsync(HttpContext httpContext);
	}
}
