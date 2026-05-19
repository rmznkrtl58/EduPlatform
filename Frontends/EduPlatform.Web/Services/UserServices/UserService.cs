using EduPlatform.Web.Models.AuthViewModels;
using Newtonsoft.Json;

namespace EduPlatform.Web.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly HttpClient _httpClient;
		public UserService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<UserViewModel> GetUser()
		{
			var response = await _httpClient.GetAsync("/api/user");
			var content=await response.Content.ReadAsStringAsync();
			var jsonContent = JsonConvert.DeserializeObject<UserViewModel>(content);
			return jsonContent!;//Authorize attribute olduğu için null hatası olmaz.
		}
	}
}
