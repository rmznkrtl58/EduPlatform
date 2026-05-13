namespace EduPlatform.Services.Basket.Settings
{
	public class RedisSettings
	{
		public const string Key = "RedisConnectionUrl";
		//Appsettings için gerekli proplarım.
		public string Host { get; set; }
		public int Port { get; set; }
	}
}