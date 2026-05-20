namespace EduPlatform.Web.Services.ClientCredentialServices
{
	public interface IClientCredentialTokenService
	{
		Task<string> GetToken();
	}
}
