using EduPlatform.Web.Options;
using Microsoft.Extensions.Options;

namespace EduPlatform.Web.Helpers
{   //Fotoğrafların Url'ni çekmek için kullanacağımız
	//Sınıfım
	public class PhotoHelper
	{
		private readonly ServiceApiOptions _serviceApiSettings;
		public PhotoHelper(IOptions<ServiceApiOptions> serviceApiSettings)
		{
			_serviceApiSettings = serviceApiSettings.Value;
		}
		public string GetPhotoStockUrl(string photoUrl)
		{
			return $"{_serviceApiSettings.PhotoStockUri}/images/{photoUrl}";
		}
	}
}
