using EduPlatform.IdentityServer.Dtos;
using EduPlatform.IdentityServer.Models;
using EduPlatform.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace EduPlatform.IdentityServer.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UserService(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<ResponseDto<ResponseMessageDto>> CreateUserAsync(CreateUserDto p)
		{
			var newUser = new ApplicationUser()
			{
				Name = p.Name,
				Email = p.Email,
				Surname = p.Surname,
				City = p.City,
				UserName = p.Username
			};
			var result = await _userManager.CreateAsync(newUser, p.Password);
			if (!result.Succeeded)
			{
				return ResponseDto<ResponseMessageDto>.Fail(result.Errors.Select(x=>x.Description).ToList(),HttpStatusCode.BadRequest.GetHashCode());
			}
			return ResponseDto<ResponseMessageDto>.Success(new ResponseMessageDto() { Message=$"Kullanıcı Kaydı Başarıyla Gerçekleşti.Kayıt Olunan Kullanıcının Id'si: {newUser.Id}"}, HttpStatusCode.Created.GetHashCode());
		}

		public async Task<ResponseDto<GetUserInfoDto>> GetUserInformationAsync(HttpContext httpContext)
		{
			//acces tokenimden gelen sub yani Id değerini aldım
			var userIdClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
			if (userIdClaim is null) return  ResponseDto<GetUserInfoDto>.Fail("Tokenden Gelen Id geçersizdir.", HttpStatusCode.BadRequest.GetHashCode());

			var user =await _userManager.FindByIdAsync(userIdClaim.Value);
			if(user is null)return ResponseDto<GetUserInfoDto>.Fail("Tokenden Gelen Id değerine ait kullanıcı yoktur.", HttpStatusCode.BadRequest.GetHashCode());

			var responseMessage = new GetUserInfoDto()
			{
				City = user.City,
				Email = user.Email,
				UserId = user.Id,
				UserName = user.UserName
			};
			return ResponseDto<GetUserInfoDto>.Success(responseMessage,HttpStatusCode.OK.GetHashCode());
		}
	}
}
